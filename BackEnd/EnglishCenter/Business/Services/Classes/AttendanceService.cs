using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Classes
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AttendanceService(IUnitOfWork unit, IMapper mapper, IUserService userService)
        {
            _unit = unit;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<Response> ChangeIsAttendedAsync(long id, bool isAttended)
        {
            var attendModel = _unit.Attendances.GetById(id);
            if (attendModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attendances",
                    Success = false
                };
            }

            var isSuccess = await _unit.Attendances.ChangeIsAttendedAsync(attendModel, isAttended);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change attend status is failed",
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

        public async Task<Response> ChangeIsLateAsync(long id, bool isLate)
        {
            var attendModel = _unit.Attendances.GetById(id);
            if (attendModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attendances",
                    Success = false
                };
            }

            var isSuccess = await _unit.Attendances.ChangeIsLateAsync(attendModel, isLate);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change late status is failed",
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

        public async Task<Response> ChangeIsLeavedAsync(long id, bool isLeaved)
        {
            var attendModel = await _unit.Attendances
                                         .Include(a => a.Enrollment)
                                         .FirstOrDefaultAsync(a => a.AttendanceId == id);
            if (attendModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attendances",
                    Success = false
                };
            }

            var isSuccess = await _unit.Attendances.ChangeIsLeavedAsync(attendModel, isLeaved);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change leave status is failed",
                    Success = false
                };
            }

            await _unit.Notifications.SendNotificationToUser(attendModel.Enrollment.UserId, attendModel.Enrollment.ClassId, new NotiDto()
            {
                Title = "Automatic Message",
                Description = $"Why are you absent from class on {DateTime.Now.ToString("MMMM dd")}? Please try your best to attend school",
                Image = "/notifications/images/automatic.svg",
                IsRead = false,
                Time = DateTime.Now,
            });

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeIsPermittedAsync(long id, bool isPermitted)
        {
            var attendModel = _unit.Attendances.GetById(id);
            if (attendModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attendances",
                    Success = false
                };
            }

            if (attendModel.IsLeaved == false)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Student isn't leaved",
                    Success = false
                };
            }

            var isSuccess = await _unit.Attendances.ChangeIsPermittedAsync(attendModel, isPermitted);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change permit status is failed",
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

        public async Task<Response> CreateAsync(AttendanceDto model)
        {
            var enrollment = _unit.Enrollment.GetById(model.EnrollId);
            if (enrollment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            var lessonModel = _unit.Lessons.GetById(model.LessonId);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            if (enrollment.ClassId != lessonModel.ClassId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Classes don't match each other",
                    Success = false
                };
            }

            var isExistRecord = _unit.Attendances.IsExist(a => a.EnrollId == model.EnrollId && a.LessonId == model.LessonId);
            if (isExistRecord)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }

            var attendModel = _mapper.Map<Attendance>(model);

            _unit.Attendances.Add(attendModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };

        }

        public async Task<Response> DeleteAsync(long id)
        {
            var attendModel = _unit.Attendances.GetById(id);
            if (attendModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attendances",
                    Success = false
                };
            }

            _unit.Attendances.Remove(attendModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<Response> GetAllAsync()
        {
            var attendModels = _unit.Attendances.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AttendanceDto>>(attendModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var attendModel = _unit.Attendances.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AttendanceDto>(attendModel),
                Success = true
            });
        }

        public async Task<Response> GetByClassAsync(string classId)
        {
            var attendModels = await _unit.Attendances
                                   .Include(s => s.Lesson)
                                   .Where(s => s.Lesson.ClassId == classId)
                                   .ToListAsync();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AttendanceDto>>(attendModels),
                Success = true
            };
        }

        public async Task<Response> GetByLessonAsync(long lessonId)
        {
            var attendModels = _unit.Attendances
                                    .Include(s => s.Enrollment)
                                    .Where(s => s.LessonId == lessonId)
                                    .ToList();

            var modelResDtos = new List<AttendanceResDto>();

            foreach (var model in attendModels)
            {
                var resDto = _mapper.Map<AttendanceResDto>(model);
                var infoRes = await _userService.GetUserFullInfoAsync(model.Enrollment.UserId);

                resDto.UserInfo = infoRes.Message as UserInfoResDto;

                modelResDtos.Add(resDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = modelResDtos,
                Success = true
            };
        }

        public async Task<Response> HandleAttendedAllAsync(long lessonId)
        {
            var attendModels = _unit.Attendances
                                    .Find(s => s.LessonId == lessonId)
                                    .ToList();

            await _unit.BeginTransAsync();

            try
            {
                foreach (var model in attendModels)
                {
                    var isChangeSuccess = await _unit.Attendances.ChangeIsAttendedAsync(model, true);
                    if (!isChangeSuccess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't change attendances",
                            Success = false
                        };
                    }
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();


                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<Response> HandleCreateByLessonAsync(long lessonId)
        {
            var lessonModel = _unit.Lessons.GetById(lessonId);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            var enrollIds = _unit.Enrollment
                               .Find(e => e.ClassId == lessonModel.ClassId &&
                                          e.StatusId == (int)EnrollEnum.Ongoing)
                               .Select(e => e.EnrollId)
                               .ToList();

            await _unit.BeginTransAsync();

            try
            {
                foreach (var enrollId in enrollIds)
                {
                    var attendDto = new AttendanceDto()
                    {
                        EnrollId = enrollId,
                        LessonId = lessonId
                    };

                    var createRes = await CreateAsync(attendDto);
                    if (!createRes.Success) return createRes;
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }

            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<Response> UpdateAsync(long id, AttendanceDto model)
        {
            var attendModel = _unit.Attendances.GetById(id);
            if (attendModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attendances",
                    Success = false
                };
            }

            try
            {
                if (model.IsAttended.HasValue)
                {
                    var changeRes = await ChangeIsAttendedAsync(id, model.IsAttended.Value);
                    if (!changeRes.Success) return changeRes;
                }

                if (model.IsPermitted.HasValue)
                {
                    var changeRes = await ChangeIsPermittedAsync(id, model.IsPermitted.Value);
                    if (!changeRes.Success) return changeRes;
                }

                if (model.IsLate.HasValue)
                {
                    var changeRes = await ChangeIsLateAsync(id, model.IsLate.Value);
                    if (!changeRes.Success) return changeRes;
                }
                if (model.IsLeaved.HasValue)
                {
                    var changeRes = await ChangeIsLeavedAsync(id, model.IsLeaved.Value);
                    if (!changeRes.Success) return changeRes;
                }

                await _unit.CompleteAsync();
                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                await _unit.RollBackTransAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
