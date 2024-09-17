using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Global.Enum;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using EnrollStatus = EnglishCenter.Global.Enum.EnrollStatus;

namespace EnglishCenter.Repositories.CourseRepositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;

        public EnrollmentRepository(EnglishCenterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response> ChangeClassAsync(long enrollmentId, string classId)
        {
            var enrollModel = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any enrollments",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isExistClass = await _context.Classes.AnyAsync(c => c.ClassId == classId);

            if (!isExistClass)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            enrollModel.ClassId = classId;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> ChangeStatusAsync(long enrollmentId, int statusId)
        {
            var enrollModel = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any enrollments",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isExistEnrollStatus = await _context.EnrollStatuses.FindAsync(statusId);
            if(isExistEnrollStatus == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enroll status",
                    Success = false
                };
            }


            enrollModel.StatusId = statusId;
            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> ChangeStatusWithClassAsync(string classId)
        {
            var classModel = await _context.Classes.FindAsync(classId);
            if(classModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any classes",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var enrollments = await _context.Enrollments.Where(e => e.ClassId == classModel.ClassId && e.StatusId == (int)EnrollStatus.Accepted).ToListAsync();
            
            foreach(var enroll in enrollments)
            {
                if(classModel.StartDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    enroll.StatusId = (int)EnrollStatus.Waiting;
                }
                else
                {
                    enroll.StatusId = (int)EnrollStatus.Ongoing;
                }
            }

            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteEnrollmentAsync(long enrollmentId)
        {
            var enrollModel = await _context.Enrollments.FindAsync(enrollmentId);
            
            if(enrollModel == null)
            {
                return new Response()
                {
                    Message = "Can't find any enrollments",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            _context.Enrollments.Remove(enrollModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> EnrollInClassAsync(EnrollmentDto model)
        {
            var classModel = await _context.Classes.FindAsync(model.ClassId);

            if(classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var isStillLearning = _context.Enrollments
                                          .Where(e => e.UserId == model.UserId && e.Class.CourseId == classModel.CourseId && e.StatusId != (int)EnrollStatus.Completed)
                                          .Any();

            

            if (isStillLearning)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "This student is still studying in the course",
                    Success = false
                };
            }

            var enrollModel = _mapper.Map<Enrollment>(model);
            enrollModel.EnrollDate = model.EnrollDate ?? DateOnly.FromDateTime(DateTime.Now);
            enrollModel.StatusId = (int) EnrollStatus.Pending;

            _context.Enrollments.Add(enrollModel);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetEnrollmentAsync(long enrollmentId)
        {
            var enrollModel = await _context.Enrollments.FindAsync(enrollmentId);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<EnrollmentDto>(enrollModel)
            };
        }

        public async Task<Response> GetEnrollmentsAsync()
        {
            var enrollModels = await _context.Enrollments.ToListAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrollModels)
            };
        }

        public async Task<Response> GetEnrollmentsAsync(string userId, string classId)
        {
            var enrollModels = await _context.Enrollments
                                            .Where(e => e.UserId == userId && e.ClassId == classId)
                                            .ToListAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrollModels),
                Success = true
            };
        }

        public async Task<Response> GetEnrollmentsWithStatusAsync(string classId, EnrollStatus status)
        {
            var enrollments = await _context.Enrollments.Where(e => e.ClassId == classId && e.StatusId == (int)status).ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrollments),
                Success = true
            };
        }

        public async Task<Response> UpdateEnrollmentAsync(long enrollmentId, EnrollmentDto model)
        {
            var enrollModel = await _context.Enrollments.FindAsync(enrollmentId);
            if(enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any enrollments"
                };
            }


            if(enrollModel.ClassId != model.ClassId)
            {
                var isExistClass = await _context.Classes.AnyAsync(c => c.ClassId == model.ClassId);

                if(!isExistClass)
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

            if(enrollModel.UserId != model.UserId)
            {
                var isExistUser = await _context.Students.AnyAsync(s => s.UserId == model.UserId);
                if (!isExistUser)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any students",
                        Success = false
                    };
                }

                enrollModel.UserId = model.UserId ?? enrollModel.UserId;
            }

            enrollModel.EnrollDate = model.EnrollDate;
            enrollModel.StatusId = model.StatusId ?? enrollModel.StatusId;
            _context.SaveChangesAsync().Wait();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }
    }
}
