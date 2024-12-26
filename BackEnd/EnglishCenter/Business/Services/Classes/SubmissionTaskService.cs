using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Classes
{
    public class SubmissionTaskService : ISubmissionTaskService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly ISubmissionFileService _fileService;

        public SubmissionTaskService(IMapper mapper, IUnitOfWork unit, ISubmissionFileService fileService)
        {
            _mapper = mapper;
            _unit = unit;
            _fileService = fileService;
        }

        public async Task<Response> ChangeDescriptionAsync(long id, string newDescription)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionTasks.ChangeDescriptionAsync(taskModel, newDescription);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change description",
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

        public async Task<Response> ChangeEndTimeAsync(long id, string endTime)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            if (!DateTime.TryParse(endTime, out var dateTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End time is invalid",
                    Success = false
                };
            }

            if (taskModel.StartTime > dateTime)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End time must be greater than start time",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionTasks.ChangeEndTimeAsync(taskModel, dateTime);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change end time",
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

        public async Task<Response> ChangeLessonAsync(long id, long lessonId)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

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

            var isChangeSuccess = await _unit.SubmissionTasks.ChangeLessonAsync(taskModel, lessonId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change lessons",
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

        public async Task<Response> ChangeStartTimeAsync(long id, string startTime)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            if (!DateTime.TryParse(startTime, out var dateTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End time is invalid",
                    Success = false
                };
            }

            if (taskModel.EndTime < dateTime)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start time must be less than end time",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionTasks.ChangeStartTimeAsync(taskModel, dateTime);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change start time",
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

        public async Task<Response> ChangeTitleAsync(long id, string newTitle)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubmissionTasks.ChangeTitleAsync(taskModel, newTitle);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change title",
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

        public async Task<Response> CreateAsync(SubmissionTaskDto model)
        {
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

            if (!DateTime.TryParse(model.StartTime, out var startTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start Time is invalid",
                    Success = false
                };
            }

            if (!DateTime.TryParse(model.EndTime, out var endTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End time is invalid",
                    Success = false
                };
            }

            if (startTime > endTime)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start time must be less than end time",
                    Success = false
                };
            }

            var taskModel = new SubmissionTask()
            {
                Title = model.Title,
                Description = model.Description,
                StartTime = startTime,
                EndTime = endTime,
                LessonId = lessonModel.LessonId
            };

            _unit.SubmissionTasks.Add(taskModel);

            await _unit.Notifications.SendNotificationToGroup(lessonModel.ClassId, new NotiDto()
            {
                Title = "Automatic Message",
                Description = $"You have a submission task. Please complete it before {endTime.ToString("MMMM dd")}",
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

        public async Task<Response> DeleteAsync(long id)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            var fileIds = _unit.SubmissionFiles
                                  .Find(s => s.SubmissionTaskId == id)
                                  .Select(s => s.SubmissionFileId)
                                  .ToList();

            foreach (var fileId in fileIds)
            {
                var res = await _fileService.DeleteAsync(fileId);
                if (!res.Success) return res;
            }

            _unit.SubmissionTasks.Remove(taskModel);

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
            var models = _unit.SubmissionTasks.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubmissionTaskResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.SubmissionTasks.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<SubmissionTaskResDto>(model),
                Success = true
            });
        }

        public async Task<Response> GetByLessonAllAsync(long lessonId)
        {
            var tasks = await _unit.SubmissionTasks
                             .Include(s => s.Lesson)
                             .Where(s => s.LessonId == lessonId)
                             .OrderByDescending(s => s.EndTime)
                             .ToListAsync();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubmissionTaskResDto>>(tasks),
                Success = true
            };
        }

        public async Task<Response> GetByLessonAsync(long lessonId)
        {
            var tasks = await _unit.SubmissionTasks
                              .Include(s => s.Lesson)
                              .Where(s => s.LessonId == lessonId && s.EndTime >= DateTime.Now)
                              .OrderByDescending(s => s.EndTime)
                              .ToListAsync();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubmissionTaskResDto>>(tasks),
                Success = true
            };
        }

        public async Task<Response> GetCurrentByClassAsync(string classId)
        {
            var tasks = await _unit.SubmissionTasks
                             .Include(s => s.Lesson)
                             .Where(s => s.Lesson.ClassId == classId && s.EndTime >= DateTime.Now)
                             .OrderByDescending(s => s.EndTime)
                             .ToListAsync();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubmissionTaskResDto>>(tasks),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, SubmissionTaskDto model)
        {
            var taskModel = _unit.SubmissionTasks.GetById(id);
            if (taskModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submission tasks",
                    Success = false
                };
            }

            if (!DateTime.TryParse(model.StartTime, out var startTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start Time is invalid",
                    Success = false
                };
            }

            if (!DateTime.TryParse(model.EndTime, out var endTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End time is invalid",
                    Success = false
                };
            }

            if (startTime > endTime)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start time must be less than end time",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (taskModel.LessonId != model.LessonId)
                {
                    var res = await ChangeLessonAsync(id, model.LessonId);
                    if (!res.Success) return res;
                }

                if (taskModel.Title != model.Title)
                {
                    var res = await ChangeTitleAsync(id, model.Title);
                    if (!res.Success) return res;
                }
                if (!string.IsNullOrEmpty(model.Description) && taskModel.Description != model.Description)
                {
                    var res = await ChangeDescriptionAsync(id, model.Description);
                    if (!res.Success) return res;
                }
                if (taskModel.StartTime != startTime)
                {
                    var res = await ChangeStartTimeAsync(id, model.StartTime);
                    if (!res.Success) return res;
                }

                if (taskModel.EndTime != endTime)
                {
                    var res = await ChangeEndTimeAsync(id, model.EndTime);
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
