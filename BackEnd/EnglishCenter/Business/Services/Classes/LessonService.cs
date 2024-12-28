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
    public class LessonService : ILessonService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IClassMaterialService _classMaterialService;
        private readonly ISubmissionTaskService _submissionTaskService;

        public LessonService(IUnitOfWork unit, IMapper mapper, IClassMaterialService classMaterialService, ISubmissionTaskService submissionTaskService)
        {
            _unit = unit;
            _mapper = mapper;
            _classMaterialService = classMaterialService;
            _submissionTaskService = submissionTaskService;
        }

        public Task<Response> IsInChargeOfClassAsync(long id, string userId)
        {
            var lessonModel = _unit.Lessons.Include(l => l.Class).FirstOrDefault(l => l.LessonId == id);
            if (lessonModel == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                });
            }

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = lessonModel.Class.TeacherId == userId,
                Success = true
            });
        }

        public async Task<Response> ChangeClassAsync(long id, string classId)
        {
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

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

            var isChangeSuccess = await _unit.Lessons.ChangeClassAsync(lessonModel, classId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change class failed",
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

        public async Task<Response> ChangeClassRoomAsync(long id, long classRoomId)
        {
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            var classRoomModel = _unit.ClassRooms.GetById(classRoomId);
            if (classRoomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Lessons.ChangeClassRoomAsync(lessonModel, classRoomId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change class room failed",
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

        public async Task<Response> ChangeDateAsync(long id, string dateOnlyStr)
        {
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            if (!DateOnly.TryParse(dateOnlyStr, out DateOnly date))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Date is invalid",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Lessons.ChangeDateAsync(lessonModel, date);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change date failed",
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

        public async Task<Response> ChangeEndPeriodAsync(long id, int endPeriod)
        {
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            if (endPeriod < 0 || endPeriod > 12)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End period is invalid",
                    Success = false
                };
            }

            if (lessonModel.StartPeriod > endPeriod)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End period must be greater than start",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Lessons.ChangeEndPeriodAsync(lessonModel, endPeriod);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change end period failed",
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

        public async Task<Response> ChangeStartPeriodAsync(long id, int startPeriod)
        {
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            if (startPeriod < 0 || startPeriod > 12)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start period is invalid",
                    Success = false
                };
            }

            if (lessonModel.EndPeriod < startPeriod)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End period must be greater than start",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Lessons.ChangeStartPeriodAsync(lessonModel, startPeriod);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change start period failed",
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

        public async Task<Response> ChangeTopicAsync(long id, string topic)
        {
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Lessons.ChangeTopicAsync(lessonModel, topic);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change topic failed",
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

        public async Task<Response> CreateAsync(LessonDto lessonModel)
        {
            var classModel = _unit.Classes.GetById(lessonModel.ClassId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            if (lessonModel.StartPeriod > lessonModel.EndPeriod)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start period must be less than End period",
                    Success = false
                };
            }

            var classRoomModel = _unit.ClassRooms.GetById(lessonModel.ClassRoomId);
            if (classRoomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            if (classRoomModel.Capacity < classModel.RegisteredNum)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Classroom capacity is not enough",
                    Success = false
                };
            }

            var lessonEntity = _mapper.Map<Lesson>(lessonModel);

            var isExistClass = _unit.Lessons
                                .Find(s => s.ClassId == lessonEntity.ClassId &&
                                            s.Date == lessonEntity.Date &&
                                            s.StartPeriod == lessonEntity.StartPeriod &&
                                            s.EndPeriod == lessonEntity.EndPeriod)
                                .Any();

            if (isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't create same lesson",
                    Success = false
                };
            }

            var isDuplicate = await _unit.Lessons.IsDuplicateAsync(lessonEntity.Date, lessonEntity.ClassRoomId, lessonEntity.StartPeriod, lessonEntity.EndPeriod);
            if (isDuplicate)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Cannot register because this schedule has been duplicated",
                    Success = false
                };
            }

            _unit.Lessons.Add(lessonEntity);
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
            var lessonModel = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            var classMaterialIds = _unit.ClassMaterials
                                        .Find(l => l.LessonId == id)
                                        .Select(s => s.ClassMaterialId)
                                        .ToList();

            foreach (var materialId in classMaterialIds)
            {
                var res = await _classMaterialService.DeleteAsync(materialId);
                if (!res.Success) return res;
            }

            var submissionIds = _unit.SubmissionTasks
                                        .Find(l => l.LessonId == id)
                                        .Select(s => s.SubmissionId)
                                        .ToList();

            foreach (var submissionId in submissionIds)
            {
                var res = await _submissionTaskService.DeleteAsync(submissionId);
                if (!res.Success) return res;
            }

            var attendances = _unit.Attendances
                                        .Find(l => l.LessonId == id)
                                        .ToList();

            _unit.Attendances.RemoveRange(attendances);
            _unit.Lessons.Remove(lessonModel);
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
            var models = _unit.Lessons.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<LessonDto>>(models),
                Success = true
            });
        }

        public async Task<Response> GetAsync(long id)
        {
            var model = await _unit.Lessons.Include(l => l.ClassRoom).FirstOrDefaultAsync(l => l.LessonId == id);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<LessonDto>(model),
                Success = true
            };
        }

        public async Task<Response> GetByEnrollAsync(long enrollId)
        {
            var enrollModel = _unit.Enrollment.GetById(enrollId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            if (enrollModel.StatusId != (int)EnrollEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You are not permitted to access this website",
                    Success = false
                };
            }

            var lessonModels = await _unit.Lessons
                                         .Include(l => l.ClassRoom)
                                         .Where(l => l.ClassId == enrollModel.ClassId)
                                         .OrderBy(l => l.Date)
                                         .ToListAsync();

            var lessonResDtos = _mapper.Map<List<LessonResDto>>(lessonModels);
            foreach (var resDto in lessonResDtos)
            {
                resDto.StartPeriodTime = _unit.Periods.GetById(resDto.StartPeriod).StartTime;
                resDto.EndPeriodTime = _unit.Periods.GetById(resDto.EndPeriod).EndTime;
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = lessonResDtos,
                Success = true
            };

        }

        public async Task<Response> GetByClassAsync(string classId)
        {
            var lessonModels = await _unit.Lessons
                                    .Include(l => l.ClassRoom)
                                    .Where(l => l.ClassId == classId)
                                    .OrderBy(l => l.Date)
                                    .ToListAsync();

            var lessonResDtos = _mapper.Map<List<LessonResDto>>(lessonModels);
            foreach (var resDto in lessonResDtos)
            {
                resDto.StartPeriodTime = _unit.Periods.GetById(resDto.StartPeriod).StartTime;
                resDto.EndPeriodTime = _unit.Periods.GetById(resDto.EndPeriod).EndTime;
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = lessonResDtos,
                Success = true
            };
        }

        public Task<Response> GetDateByClassAsync(string classId)
        {
            var lessonModels = _unit.Lessons
                                          .Find(l => l.ClassId == classId && l.Date <= DateOnly.FromDateTime(DateTime.Now))
                                          .OrderByDescending(l => l.Date)
                                          .Select(l => new
                                          {
                                              LessonId = l.LessonId,
                                              Date = l.Date.ToString("dd/MM/yyyy")
                                          })
                                          .ToList();
            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = lessonModels,
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, LessonDto lessonModel)
        {
            var lessonEntity = _unit.Lessons.GetById(id);
            if (lessonModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any lessons",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (lessonEntity.ClassRoomId != lessonModel.ClassRoomId)
                {
                    var res = await ChangeClassRoomAsync(id, lessonModel.ClassRoomId);
                    if (!res.Success) return res;
                }

                if (lessonEntity.Date.ToString() != lessonModel.Date)
                {
                    var res = await ChangeDateAsync(id, lessonModel.Date);
                    if (!res.Success) return res;
                }

                if (lessonEntity.StartPeriod != lessonModel.StartPeriod)
                {
                    var res = await ChangeStartPeriodAsync(id, lessonModel.StartPeriod);
                    if (!res.Success) return res;
                }

                if (lessonEntity.EndPeriod != lessonModel.EndPeriod)
                {
                    var res = await ChangeEndPeriodAsync(id, lessonModel.EndPeriod);
                    if (!res.Success) return res;
                }

                if (lessonEntity.Topic != lessonModel.Topic)
                {
                    var res = await ChangeTopicAsync(id, lessonModel.Topic ?? "");
                    if (!res.Success) return res;
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
