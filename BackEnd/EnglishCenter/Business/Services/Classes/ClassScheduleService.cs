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
    public class ClassScheduleService : IClassScheduleService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;

        public ClassScheduleService(IUnitOfWork unit, IMapper mapper, ILessonService lessonService)
        {
            _unit = unit;
            _mapper = mapper;
            _lessonService = lessonService;
        }

        public async Task<Response> ChangeClassAsync(long id, string classId)
        {
            var classSchedule = _unit.ClassSchedules.GetById(id);
            if (classSchedule == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
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

            var isChangeSuccess = await _unit.ClassSchedules.ChangeClassAsync(classSchedule, classId);
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
            var classSchedule = _unit.ClassSchedules.Include(s => s.Class).FirstOrDefault(s => s.ScheduleId == id);
            if (classSchedule == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
                    Success = false
                };
            }

            var classRoomModel = _unit.ClassRooms.GetById(classRoomId);
            if (classRoomModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            if (classSchedule.Class.RegisteredNum > classRoomModel.Capacity)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Classroom capacity is not enough",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassSchedules.ChangeClassRoomAsync(classSchedule, classRoomId);
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

        public async Task<Response> ChangeDayOfWeekAsync(long id, int dayOfWeek)
        {
            var classSchedule = _unit.ClassSchedules.GetById(id);
            if (classSchedule == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
                    Success = false
                };
            }

            if (dayOfWeek < 0 || dayOfWeek >= 7)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Out of range day of week",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassSchedules.ChangeDayOfWeekAsync(classSchedule, dayOfWeek);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change day of week failed",
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

        public async Task<Response> ChangeEndPeriodAsync(long id, int end)
        {
            var classSchedule = _unit.ClassSchedules.GetById(id);
            if (classSchedule == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
                    Success = false
                };
            }

            if (classSchedule.StartPeriod > end)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End period must be greater than start",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassSchedules.ChangeEndPeriodAsync(classSchedule, end);
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

        public async Task<Response> ChangeStartPeriodAsync(long id, int start)
        {
            var classSchedule = _unit.ClassSchedules.GetById(id);
            if (classSchedule == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
                    Success = false
                };
            }

            if (classSchedule.EndPeriod < start)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start period must be less than end",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ClassSchedules.ChangeStartPeriodAsync(classSchedule, start);
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

        public async Task<Response> CreateAsync(ClassScheduleDto classSchedule)
        {
            var classModel = _unit.Classes.GetById(classSchedule.ClassId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var classRoom = _unit.ClassRooms.GetById(classSchedule.ClassRoomId);
            if (classRoom == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class rooms",
                    Success = false
                };
            }

            if (classRoom.Capacity < classModel.RegisteredNum)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Classroom capacity is not enough",
                    Success = false
                };
            }

            bool isActive = false;
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (classModel.Status != (int)ClassEnum.End && classModel.StartDate <= currentDate && currentDate <= classModel.EndDate)
            {
                isActive = true;
            }

            var isDuplicate = await _unit.ClassSchedules.IsDuplicateAsync(classSchedule.DayOfWeek, classSchedule.ClassRoomId, classSchedule.StartPeriod, classSchedule.EndPeriod);
            if (isDuplicate)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Cannot register because this schedule has been duplicated",
                    Success = false
                };
            }

            var isDuplicateScheduleOfTeacher = await _unit.ClassSchedules.IsDuplicateTeacherAsync(classSchedule.DayOfWeek, classSchedule.StartPeriod, classSchedule.EndPeriod, classModel.TeacherId);
            if (isDuplicateScheduleOfTeacher)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Teacher's schedule is already booked so cannot register",
                    Success = false
                };
            }

            var isExist = _unit.ClassSchedules
                               .IsExist(s => s.ClassId == classSchedule.ClassId &&
                                             s.DayOfWeek == classSchedule.DayOfWeek &&
                                             s.IsActive == true);

            if (isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't register same day",
                    Success = false
                };
            }

            var scheduleEntity = _mapper.Map<ClassSchedule>(classSchedule);
            scheduleEntity.IsActive = isActive;

            _unit.ClassSchedules.Add(scheduleEntity);

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
            var classSchedule = _unit.ClassSchedules.GetById(id);
            if (classSchedule == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
                    Success = false
                };
            }

            var classModel = _unit.Classes.GetById(classSchedule.ClassId);
            if (classModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            if (classModel.StartDate.HasValue == false || classModel.EndDate.HasValue == false)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Classes have no start or end times.",
                    Success = false
                };
            }

            for (DateOnly current = classModel.StartDate.Value; current <= classModel.EndDate.Value; current = current.AddDays(1))
            {
                if (classSchedule.DayOfWeek == (int)current.DayOfWeek)
                {

                    var lessonModel = _unit.Lessons.Find(s => s.ClassId == classSchedule.ClassId &&
                                                              s.Date == current &&
                                                              s.StartPeriod == classSchedule.StartPeriod &&
                                                              s.EndPeriod == classSchedule.EndPeriod)
                                                   .FirstOrDefault();


                    if (lessonModel != null)
                    {
                        var deleteRes = await _lessonService.DeleteAsync(lessonModel.LessonId);
                        if (!deleteRes.Success) return deleteRes;
                    }
                }
            }

            _unit.ClassSchedules.Remove(classSchedule);
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
            var models = _unit.ClassSchedules.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ClassScheduleDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.ClassSchedules.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ClassScheduleDto>(model),
                Success = true
            });
        }

        public async Task<Response> GetByClassAsync(string classId)
        {
            var scheduleModels = await _unit.ClassSchedules
                                      .Include(c => c.ClassRoom)
                                      .Where(c => c.ClassId == classId && c.IsActive == true)
                                      .ToListAsync();

            var scheduleResDtos = new List<ClassScheduleResDto>();

            foreach (var schedule in scheduleModels)
            {
                var startPeriod = _unit.Periods.GetById(schedule.StartPeriod);
                var endPeriod = _unit.Periods.GetById(schedule.EndPeriod);
                var resDto = new ClassScheduleResDto();
                resDto.ScheduleId = schedule.ScheduleId;
                resDto.ClassId = schedule.ClassId;
                resDto.DayOfWeek = schedule.DayOfWeek;
                resDto.DayOfWeekStr = ((DayOfWeek)schedule.DayOfWeek).ToString();
                resDto.StartPeriod = schedule.StartPeriod;
                resDto.EndPeriod = schedule.EndPeriod;
                resDto.StartPeriodStr = startPeriod.StartTime + "-" + startPeriod.EndTime;
                resDto.EndPeriodStr = endPeriod.StartTime + "-" + endPeriod.EndTime;
                resDto.ClassRoomInfo = _mapper.Map<ClassRoomDto>(schedule.ClassRoom);
                scheduleResDtos.Add(resDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = scheduleResDtos,
                Success = true
            };
        }

        public Task<Response> GetDayOfWeekAsync()
        {
            var dayOfWeeks = Enum.GetValues(typeof(DayOfWeek))
                           .Cast<DayOfWeek>()
                           .Select(type => new KeyValuePair<string, int>(type.ToString(), (int)type))
                           .ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = dayOfWeeks,
                Success = true
            });
        }

        public async Task<Response> HandleCreateLessonAsync(string classId)
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

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (classModel.Status == (int)ClassEnum.End || classModel.EndDate < currentDate)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The class has ended and no more classes can be created.",
                    Success = false
                };
            }

            if (classModel.StartDate.HasValue == false || classModel.EndDate.HasValue == false)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Classes have no start or end times.",
                    Success = false
                };
            }

            var schedules = _unit.ClassSchedules
                                 .Find(s => s.ClassId == classId && s.IsActive == true);

            await _unit.BeginTransAsync();

            try
            {
                for (DateOnly current = classModel.StartDate.Value; current <= classModel.EndDate.Value; current = current.AddDays(1))
                {
                    if (schedules.Any(s => s.DayOfWeek == (int)current.DayOfWeek))
                    {
                        var schedule = schedules.First(s => s.DayOfWeek == (int)current.DayOfWeek);
                        var lessonDto = new LessonDto()
                        {
                            ClassId = schedule.ClassId,
                            ClassRoomId = schedule.ClassRoomId,
                            Date = current.ToString(),
                            StartPeriod = schedule.StartPeriod,
                            EndPeriod = schedule.EndPeriod,
                            Topic = ""
                        };

                        var isExistLesson = _unit.Lessons.Find(s => s.ClassId == lessonDto.ClassId &&
                                                                    s.Date == current &&
                                                                    s.StartPeriod == lessonDto.StartPeriod &&
                                                                    s.EndPeriod == lessonDto.EndPeriod)
                                                                    .Any();

                        if (!isExistLesson)
                        {
                            var createRes = await _lessonService.CreateAsync(lessonDto);
                            if (!createRes.Success) return createRes;
                        }

                    }
                }

                await _unit.CommitTransAsync();
                await _unit.CompleteAsync();

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

        public async Task<Response> UpdateAsync(long id, ClassScheduleDto classSchedule)
        {
            var scheduleModel = _unit.ClassSchedules.GetById(id);
            if (scheduleModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any class schedules",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (scheduleModel.ClassRoomId != classSchedule.ClassRoomId)
                {
                    var response = await ChangeClassRoomAsync(id, classSchedule.ClassRoomId);
                    if (!response.Success) return response;
                }

                if (scheduleModel.ClassId != classSchedule.ClassId)
                {
                    var response = await ChangeClassAsync(id, classSchedule.ClassId);
                    if (!response.Success) return response;
                }

                if (scheduleModel.DayOfWeek != classSchedule.DayOfWeek)
                {
                    var response = await ChangeDayOfWeekAsync(id, classSchedule.DayOfWeek);
                    if (!response.Success) return response;
                }

                if (scheduleModel.StartPeriod != classSchedule.StartPeriod)
                {
                    var response = await ChangeStartPeriodAsync(id, classSchedule.StartPeriod);
                    if (!response.Success) return response;
                }

                if (scheduleModel.EndPeriod != classSchedule.EndPeriod)
                {
                    var response = await ChangeEndPeriodAsync(id, classSchedule.StartPeriod);
                    if (!response.Success) return response;
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
