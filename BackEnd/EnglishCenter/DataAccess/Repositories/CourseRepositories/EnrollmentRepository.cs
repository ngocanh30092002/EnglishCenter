using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using EnrollEnum = EnglishCenter.Presentation.Global.Enum.EnrollEnum;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment> ,IEnrollmentRepository
    {

        public EnrollmentRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<List<Enrollment>> GetAsync(string userId)
        {
            var enrolls = await context.Enrollments.Where(c => c.UserId == userId).ToListAsync(); 

            return enrolls;
        }

        public async Task<List<Enrollment>> GetAsync(string userId, string classId)
        {
            var enrollModels = await context.Enrollments
                                            .Where(e => e.UserId == userId && e.ClassId == classId)
                                            .ToListAsync();

            return enrollModels;
        }

        public async Task<List<Enrollment>> GetAsync(string classId, EnrollEnum status)
        {
            var enrollments = await context.Enrollments.Where(e => e.ClassId == classId && e.StatusId == (int)status).ToListAsync();

            return enrollments;
        }

        public async Task<List<Enrollment>> GetByTeacherAsync(string userId)
        {
            var enrolls = await context.Enrollments.Include(c => c.Class)
                                            .Where(c => c.Class.TeacherId == userId)
                                            .ToListAsync();

            return enrolls;
        }

        public async Task<List<Enrollment>> GetByTeacherAsync(string userId, string classId)
        {
            var enrolls = await context.Enrollments.Include(c => c.Class)
                                            .Where(c => c.Class.TeacherId == userId &&
                                                        c.ClassId == classId)
                                            .ToListAsync();
            return enrolls;
        }
        
        public async Task<int> GetHighestPreScoreAsync(string userId, string courseId)
        {
            var enrolls = await context.Enrollments.Include(c => c.Class)
                                        .Where(c => c.Class.CourseId == courseId &&
                                                    c.UserId == userId &&
                                                    c.StatusId == (int) EnrollEnum.Completed)
                                        .ToListAsync();

            var maxScore = (from e in enrolls
                           join s in context.ScoreHistories
                           on e.ScoreHisId equals s.ScoreHisId
                           select s.FinalPoint).Max();

            return maxScore ?? 0;
        }

        public async Task<bool> ChangeClassAsync(Enrollment enroll, string classId)
        {
            if (enroll == null) return false;

            var classModel = await context.Classes.FindAsync(classId);
            if (classModel == null) return false;

            enroll.ClassId = classModel.ClassId;

            return true;
        }

        public Task<bool> ChangeDateAsync(Enrollment enroll, DateOnly time)
        {
            if (enroll == null) return Task.FromResult(false);

            enroll.EnrollDate = time;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeStatusAsync(Enrollment enroll, EnrollEnum status)
        {
            if (enroll == null) return Task.FromResult(false);

            enroll.StatusId = (int)status;  
            return Task.FromResult(true);
        }
        
        public async Task<bool> ChangeStudentAsync(Enrollment enroll, string userId)
        {
            if (enroll == null) return false;

            var studentModel = await context.Students.FindAsync(userId);
            if (studentModel == null) return false;

            var classModel = await context.Classes.FindAsync(enroll.ClassId);
            if (classModel == null) return false;

            var isStillLearning = context.Enrollments.Include(c => c.Class)
                                                    .Where(c => c.Class.CourseId == classModel.CourseId &&
                                                                c.UserId == studentModel.UserId && 
                                                                c.StatusId != (int)EnrollEnum.Completed &&
                                                                c.StatusId != (int)EnrollEnum.Rejected)
                                                    .Any();

            if (isStillLearning) return false;

            enroll.UserId = userId;
            return true;
        }

        public async Task<bool> HandleAcceptedAsync(string classId)
        {
            var classModel = await context.Classes.FindAsync(classId);
            if (classModel == null) return false;

            var enrolls = await context.Enrollments
                                    .Where(e => e.StatusId == (int)EnrollEnum.Accepted && e.ClassId == classModel.ClassId)
                                    .ToListAsync();
            
            foreach(var enroll in enrolls)
            {
                enroll.StatusId = (int)EnrollEnum.Waiting;
            }

            return true;
        }

        public Task<bool> HandleAcceptedAsync(Enrollment enroll)
        {
            if (enroll == null) return Task.FromResult(false);
            if (enroll.StatusId != (int)EnrollEnum.Accepted) Task.FromResult(false);

            enroll.StatusId = (int)EnrollEnum.Waiting;
            return Task.FromResult(true);
        }

        public async Task<bool> HandleStartClassAsync(string classId, Course preCourse)
        {
            var classModel = await context.Classes.FindAsync(classId);
            if (classModel == null) return false;

            var enrolls = await GetAsync(classId, EnrollEnum.Waiting);

            foreach (var enroll in enrolls)
            {
                enroll.StatusId = (int)EnrollEnum.Ongoing;

                var scoreHis = new ScoreHistory() { EntrancePoint = 0, MidtermPoint = 0, FinalPoint = 0};
                if(preCourse != null)
                {
                    scoreHis.EntrancePoint = await GetHighestPreScoreAsync(enroll.UserId, preCourse.CourseId);
                }

                enroll.ScoreHis = scoreHis;
            }

            return true;
        }

        public async Task<bool> HandleEndClassAsync(string classId)
        {
            var classModel = await context.Classes.FindAsync(classId);
            if(classModel == null) return false;

            var enrolls = await GetAsync(classId, EnrollEnum.Ongoing);

            foreach(var enroll in enrolls)
            {
                enroll.StatusId = (int)EnrollEnum.Completed;
            }

            return true;
        }
        
        public Task<bool> HandleRejectByTeacherAsync(Enrollment enrollModel)
        {
            if(enrollModel == null) return Task.FromResult(false);
            if (enrollModel.StatusId != (int)EnrollEnum.Pending) return Task.FromResult(false);

            enrollModel.StatusId = (int)EnrollEnum.Rejected;

            return Task.FromResult(true);
        }

        public async Task<Response> UpdateAsync(long enrollmentId, EnrollmentDto model)
        {
            var enrollModel = await context.Enrollments.FindAsync(enrollmentId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any enrollments"
                };
            }


            if (enrollModel.ClassId != model.ClassId)
            {
                var isExistClass = await context.Classes.AnyAsync(c => c.ClassId == model.ClassId);

                if (!isExistClass)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any classes",
                        Success = false
                    };
                }

                enrollModel.ClassId = model.ClassId;
            }

            if (enrollModel.UserId != model.UserId)
            {
                var isExistUser = await context.Students.AnyAsync(s => s.UserId == model.UserId);
                if (!isExistUser)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any students",
                        Success = false
                    };
                }


                var classModel = await context.Classes.FindAsync(enrollModel.ClassId);

                var isStillPending = await context.Enrollments
                                                .Include(e => e.Class)
                                                .AnyAsync(e => e.Class.CourseId == classModel!.CourseId &&
                                                               e.UserId == model.UserId &&
                                                               e.StatusId != (int)EnrollEnum.Completed);
                if (isStillPending)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "This student is still studying in the course",
                        Success = false
                    };
                }
                enrollModel.UserId = model.UserId ?? enrollModel.UserId;
            }

            enrollModel.EnrollDate = model.EnrollDate.HasValue ? model.EnrollDate : DateOnly.FromDateTime(DateTime.Now);
            enrollModel.StatusId = model.StatusId ?? enrollModel.StatusId;

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }
    }
}
