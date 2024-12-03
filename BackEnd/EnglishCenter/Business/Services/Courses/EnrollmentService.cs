using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Courses
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public EnrollmentService(IMapper mapper, IUnitOfWork unit)
        {
            _unit = unit;
            _mapper = mapper;
        }

        private Task<bool> IsStillLearningAsync(string userId, string classId)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null) return Task.FromResult(false);

            var isStillLearning = _unit.Enrollment.Include(c => c.Class)
                                            .Where(c => c.Class.CourseId == classModel.CourseId &&
                                                        c.UserId == userId &&
                                                        c.StatusId != (int)EnrollEnum.Completed &&
                                                        c.StatusId != (int)EnrollEnum.Rejected)
                                            .Any();


            return Task.FromResult(isStillLearning);
        }

        private Task<bool> IsValidChangeAsync(string classId)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null) return Task.FromResult(false);

            if (classModel.RegisteredNum >= classModel.MaxNum) return Task.FromResult(false);
            if (classModel.EndDate <= DateOnly.FromDateTime(DateTime.Now)) return Task.FromResult(false);

            return Task.FromResult(true);
        }

        private async Task<Response> CreateWithNoPreCourseAsync(EnrollmentDto model, Class classModel)
        {
            var isLearning = await IsStillLearningAsync(model.UserId!, classModel.ClassId);
            if (isLearning)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't register for this class because another class is available",
                    Success = false
                };
            }

            var enrollModel = _mapper.Map<Enrollment>(model);
            enrollModel.StatusId = (int)EnrollEnum.Pending;
            enrollModel.EnrollDate = DateOnly.FromDateTime(DateTime.Now);
            enrollModel.UpdateTime = DateTime.Now;

            classModel.RegisteringNum++;
            _unit.Enrollment.Add(enrollModel);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        private async Task<Response> CreateWithPreCourseAsync(EnrollmentDto model, Course preCourse, Course currentCourse, Class classModel)
        {
            var highestScore = await _unit.Enrollment.GetHighestScoreAsync(model.UserId!, preCourse.CourseId);

            if (highestScore < currentCourse.EntryPoint)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You don't meet the requirements to register for this class.",
                    Success = false
                };
            }

            var isLearning = await IsStillLearningAsync(model.UserId!, model.ClassId);
            if (isLearning)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't register for this class because another class is available",
                    Success = false
                };
            }

            var enrollModel = _mapper.Map<Enrollment>(model);
            enrollModel.StatusId = (int)EnrollEnum.Pending;
            enrollModel.EnrollDate = DateOnly.FromDateTime(DateTime.Now);
            enrollModel.UpdateTime = DateTime.Now;

            classModel.RegisteringNum++;
            _unit.Enrollment.Add(enrollModel);

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<bool> IsEnrollmentOfStudentAsync(string userId, long enrollmentId)
        {
            var enroll = _unit.Enrollment.GetById(enrollmentId);
            if (enroll == null) return Task.FromResult(false);

            return Task.FromResult(enroll.UserId == userId);
        }

        public Task<Response> GetAllAsync()
        {
            var enrollments = _unit.Enrollment.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrollments),
                Success = true
            });
        }

        public Task<Response> GetAsync(long enrollmentId)
        {
            var enrollment = _unit.Enrollment.GetById(enrollmentId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<EnrollmentDto>(enrollment),
                Success = true
            });
        }

        public Task<Response> GetAsync(string userId, long enrollmentId)
        {
            var enrollment = _unit.Enrollment
                                  .Include(e => e.User)
                                  .Include(e => e.Class)
                                  .ThenInclude(c => c.Teacher)
                                  .FirstOrDefault(e => e.EnrollId == enrollmentId);

            if (enrollment == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                });
            }

            if (enrollment.UserId != userId)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You can't access other people's information",
                    Success = false
                });
            }

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<EnrollResDto>(enrollment),
                Success = true
            });
        }

        public async Task<Response> GetAsync(string userId)
        {
            var isExistUser = _unit.Students.IsExist(s => s.UserId == userId);
            if (!isExistUser)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Students",
                    Success = false
                };
            }

            var enrolls = await _unit.Enrollment.GetAsync(userId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollResDto>>(enrolls),
                Success = true
            };
        }

        public async Task<Response> GetByClassAsync(string classId)
        {
            var enrollments = await _unit.Enrollment
                                        .Include(e => e.Status)
                                        .Include(e => e.User)
                                        .ThenInclude(s => s.User)
                                        .Where(e => e.ClassId == classId && e.StatusId == (int)EnrollEnum.Ongoing)
                                        .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollResDto>>(enrollments),
                Success = true
            };
        }

        public async Task<Response> GetAsync(string classId, int statusId)
        {
            if (!Enum.IsDefined(typeof(EnrollEnum), statusId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Enroll status invalid"
                };
            }

            var enrollments = await _unit.Enrollment.GetAsync(classId, (EnrollEnum)statusId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollResDto>>(enrollments),
                Success = true
            };
        }

        public async Task<Response> GetAsync(string userId, string classId)
        {
            var enrollments = await _unit.Enrollment.GetAsync(userId, classId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrollments),
                Success = true
            };

        }

        public async Task<Response> GetCurrentClassesByStudentAsync(string userId)
        {
            var isExistUser = _unit.Students.IsExist(s => s.UserId == userId);
            if (!isExistUser)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var enrolls = await _unit.Enrollment.GetCurrentClassesByStudentAsync(userId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrolls),
                Success = true
            };
        }

        public async Task<Response> GetByTeacherAsync(string userId)
        {
            var teacherModel = _unit.Teachers.GetById(userId);
            if (teacherModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers",
                    Success = false
                };
            }

            var enrolls = await _unit.Enrollment.GetByTeacherAsync(userId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrolls),
                Success = true
            };
        }

        public async Task<Response> GetByTeacherAsync(string userId, string classId)
        {
            var teacherModel = _unit.Teachers.GetById(userId);
            if (teacherModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any teachers",
                    Success = false
                };
            }

            var enrolls = await _unit.Enrollment.GetByTeacherAsync(userId, classId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollmentDto>>(enrolls),
                Success = true
            };
        }

        public async Task<Response> GetByCourseAsync(string userId, string courseId)
        {
            var isExistUser = _unit.Students.IsExist(s => s.UserId == userId);
            if (!isExistUser)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == courseId);
            if (!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var enroll = await _unit.Enrollment.GetByCourseAsync(userId, courseId);
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<EnrollResDto>(enroll),
                Success = true
            };
        }

        public async Task<Response> GetHisEnrollsByCourseAsync(string userId, string courseId)
        {
            var isExistUser = _unit.Students.IsExist(s => s.UserId == userId);
            if (!isExistUser)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students",
                    Success = false
                };
            }

            var isExistCourse = _unit.Courses.IsExist(c => c.CourseId == courseId);
            if (!isExistCourse)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            var enrolls = await _unit.Enrollment.GetAsync(userId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EnrollResDto>>(enrolls.Where(e => e.Class.CourseId == courseId).OrderByDescending(e => e.UpdateTime)),
                Success = true
            };
        }

        public async Task<Response> HandleStartClassAsync(string classId)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            if (classModel.StartDate > DateOnly.FromDateTime(DateTime.Now))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Too soon to start this class",
                    Success = false
                };
            }

            var courseModel = _unit.Courses.GetById(classModel.CourseId);
            var preCourse = await _unit.Courses.GetPreviousAsync(courseModel);

            var isSuccess = await _unit.Enrollment.HandleStartClassAsync(classId, preCourse);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Classes.ChangeStatusAsync(classModel, ClassEnum.Opening);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change class status failed",
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleEndClassAsync(string classId)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            if (classModel.EndDate > DateOnly.FromDateTime(DateTime.Now))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Too soon to end this class",
                    Success = false
                };
            }

            var isSuccess = await _unit.Enrollment.HandleEndClassAsync(classId);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Classes.ChangeStatusAsync(classModel, ClassEnum.End);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change class status failed",
                    Success = false
                };
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleAcceptedAsync(long enrollmentId)
        {
            var enrollModel = _unit.Enrollment.Include(e => e.Class).FirstOrDefault(e => e.EnrollId == enrollmentId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            var classModel = _unit.Classes.GetById(enrollModel.ClassId);

            var isSuccess = await _unit.Enrollment.HandleAcceptedAsync(enrollModel);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            classModel.RegisteringNum--;
            classModel.RegisteredNum++;

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleAcceptedAsync(string classId)
        {
            var classModel = _unit.Classes.GetById(classId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var enrollNum = _unit.Enrollment.Find(e => e.ClassId == classId && e.StatusId == (int)EnrollEnum.Pending).Count();

            var isSuccess = await _unit.Enrollment.HandleAcceptedAsync(classId);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            classModel.RegisteringNum = classModel.RegisteringNum - enrollNum;
            classModel.RegisteredNum = classModel.RegisteredNum + enrollNum;

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleRejectAsync(long enrollmentId)
        {
            var enrollModel = _unit.Enrollment.GetById(enrollmentId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollment",
                    Success = false
                };
            }

            if (enrollModel.StatusId != (int)EnrollEnum.Pending && enrollModel.StatusId != (int)EnrollEnum.Waiting)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't be rejected with current statuses",
                    Success = false
                };
            }

            if (enrollModel.ScoreHisId.HasValue)
            {
                var scoreHisModel = _unit.ScoreHis.GetById(enrollModel.ScoreHisId.Value);
                _unit.ScoreHis.Remove(scoreHisModel);
            }

            enrollModel.StatusId = (int)EnrollEnum.Rejected;

            var classModel = _unit.Classes.GetById(enrollModel.ClassId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            if (enrollModel.StatusId == (int)EnrollEnum.Pending)
            {
                classModel.RegisteringNum--;
            }
            else
            {
                classModel.RegisteredNum--;
            }

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleRejectByTeacherAsync(long enrollmentId)
        {
            var enrollModel = _unit.Enrollment.GetById(enrollmentId);
            int statusId = enrollModel.StatusId ?? 0;

            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "",
                    Success = false
                };
            }

            if (statusId != (int)EnrollEnum.Pending && statusId != (int)EnrollEnum.Waiting)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't be rejected with current statuses",
                    Success = false
                };
            }

            var isSuccess = await _unit.Enrollment.HandleRejectByTeacherAsync(enrollModel);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var classModel = _unit.Classes.GetById(enrollModel.ClassId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            if (statusId == (int)EnrollEnum.Pending)
            {
                classModel.RegisteringNum--;
            }
            else
            {
                classModel.RegisteredNum--;
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleChangeClassAsync(long enrollmentId, string classId)
        {
            var enrollModel = _unit.Enrollment.GetById(enrollmentId);
            if (enrollModel.StatusId != (int)EnrollEnum.Pending && enrollModel.StatusId != (int)EnrollEnum.Waiting)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change class",
                    Success = false
                };
            }

            var isValid = await IsValidChangeAsync(classId);
            if (!isValid)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var preClass = _unit.Classes.GetById(enrollModel.ClassId);
            var nextClass = _unit.Classes.GetById(classId);

            if (enrollModel.StatusId == (int)EnrollEnum.Pending)
            {
                var isSuccess = await _unit.Enrollment.ChangeClassAsync(enrollModel, classId);

                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false
                    };
                }

                preClass.RegisteringNum--;
                nextClass.RegisteringNum++;
            }
            else
            {
                var isSuccess = await _unit.Enrollment.ChangeClassAsync(enrollModel, classId);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false
                    };
                }

                isSuccess = await _unit.Enrollment.ChangeStatusAsync(enrollModel, EnrollEnum.Pending);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Success = false
                    };
                }

                preClass.RegisteredNum--;
                nextClass.RegisteringNum++;
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> CreateAsync(EnrollmentDto model)
        {
            var classModel = _unit.Classes.GetById(model.ClassId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (classModel.Status == (int)ClassEnum.End || classModel.EndDate < currentDate)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't register for this class because it has ended",
                    Success = false
                };
            }

            if (classModel.StartDate > currentDate)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't register for this class because it hasn't started yet.",
                    Success = false
                };
            }


            var studentModel = _unit.Students.GetById(model.UserId ?? "");
            if (studentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            bool isFull = classModel.RegisteredNum >= classModel.MaxNum;
            if (isFull)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "This class is full",
                    Success = false
                };
            }

            var courseModel = _unit.Courses.GetById(classModel.CourseId);
            var preCourse = await _unit.Courses.GetPreviousAsync(courseModel);

            if (preCourse == null)
            {
                return await CreateWithNoPreCourseAsync(model, classModel);
            }
            else
            {
                return await CreateWithPreCourseAsync(model, preCourse, courseModel, classModel);
            }
        }

        public async Task<Response> DeleteAsync(long enrollmentId)
        {
            var enrollmentModel = _unit.Enrollment.GetById(enrollmentId);

            if (enrollmentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments"
                };
            }

            if (enrollmentModel.ScoreHisId.HasValue)
            {
                var scoreHisModel = _unit.ScoreHis.GetById(enrollmentModel.ScoreHisId.Value);

                _unit.ScoreHis.Remove(scoreHisModel);
            }

            var classModel = _unit.Classes.GetById(enrollmentModel.ClassId);

            if (enrollmentModel.StatusId == (int)EnrollEnum.Pending)
            {
                classModel.RegisteringNum--;
            }
            else if (enrollmentModel.StatusId != (int)EnrollEnum.Rejected)
            {
                classModel.RegisteredNum--;
            }

            _unit.Enrollment.Remove(enrollmentModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };

        }

        public async Task<Response> UpdateAsync(long enrollmentId, EnrollmentDto model)
        {
            var response = await _unit.Enrollment.UpdateAsync(enrollmentId, model);

            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }
    }
}
