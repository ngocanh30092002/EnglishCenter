using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.HomeworkTasks
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public HomeworkService(IUnitOfWork unit, IMapper mapper) 
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<bool> IsInChargeClass(string userId, long homeId)
        {
            var homeModel = await _unit.Homework.Include(h => h.Class)
                                                .FirstOrDefaultAsync(h => h.HomeworkId == homeId);
            
            return await _unit.Homework.IsInChargeAsync(homeModel, userId);
        }
        public async Task<Response> ChangeClassAsync(long id, string classId)
        {
            var homeModel = _unit.Homework.GetById(id);
            if(homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            var isExistClass = _unit.Classes.IsExist(c => c.ClassId == classId && c.Status == (int) ClassEnum.Opening);
            if (!isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes with ongoing status",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Homework.ChangeClassAsync(homeModel, classId);
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

        public async Task<Response> ChangeEndTimeAsync(long id, string endTime)
        {
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if(!DateTime.TryParse(endTime, out DateTime dateTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Input time isn't valid",
                    Success = false
                };
            }
            
            var isChangeSuccess = await _unit.Homework.ChangeEndTimeAsync(homeModel, dateTime);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change end time failed",
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

        public async Task<Response> ChangeLateSubmitDaysAsync(long id, int days)
        {
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Homework.ChangeLateSubmitDaysAsync(homeModel, days);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change late submit days failed",
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

        public async Task<Response> ChangePercentageAsync(long id, int percentage)
        {
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if(percentage < 0 || percentage > 100)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Percentage must be between 0 - 100",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Homework.ChangePercentageAsync(homeModel, percentage);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change percentage failed",
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
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if (!DateTime.TryParse(startTime, out DateTime dateTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Input time isn't valid",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Homework.ChangeStartTimeAsync(homeModel, dateTime);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change end time failed",
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

        public async Task<Response> ChangeTimeAsync(long id, string time)
        {
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if (!TimeOnly.TryParse(time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            var isChangeSuccess = await _unit.Homework.ChangeTimeAsync(homeModel, timeOnly);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change time failed",
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

        public async Task<Response> ChangeTitleAsync(long id, string title)
        {
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.Homework.ChangeTitleAsync(homeModel, title);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change title failed",
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

        public async Task<Response> CreateAsync(HomeworkDto model)
        {
            if (!DateTime.TryParse(model.EndTime,out DateTime endTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "End time isn't valid",
                    Success = false
                };
            }

            if (!DateTime.TryParse(model.StartTime, out DateTime startTime))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start time isn't valid",
                    Success = false
                };
            }

            if(startTime > endTime)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Start time must be less than end time",
                    Success = false
                };
            }

            var isExistClass = _unit.Classes.IsExist(c => c.ClassId == model.ClassId && c.Status == (int)ClassEnum.Opening);
            if (!isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes with opening status",
                    Success = false
                };
            }

            if(model.LateSubmitDays.HasValue && model.LateSubmitDays < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Late submit days isn't valid",
                    Success = false
                };
            }

            if (!TimeOnly.TryParse(model.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format",
                    Success = false
                };
            }

            if(timeOnly == TimeOnly.MinValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "You need to set a time for your homework.",
                    Success = false
                };
            }

            var homeModel = new Homework();
            homeModel.ClassId = model.ClassId;
            homeModel.Title = model.Title;
            homeModel.AchievedPercentage = model.Achieved_Percentage;
            homeModel.Time = timeOnly;
            homeModel.ExpectedTime = TimeOnly.MinValue;
            homeModel.StartTime = startTime;
            homeModel.EndTime = endTime;
            homeModel.LateSubmitDays = model.LateSubmitDays.HasValue ? model.LateSubmitDays.Value : 0;

            _unit.Homework.Add(homeModel);
            
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
            var homeModel = _unit.Homework.GetById(id);
            if(homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            _unit.Homework.Remove(homeModel);
            
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
            var models = _unit.Homework.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HomeworkResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.Homework.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HomeworkResDto>(model),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, HomeworkDto model)
        {
            var homeModel = _unit.Homework.GetById(id);
            if (homeModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if(homeModel.ClassId != model.ClassId)
                {
                    var changeResponse = await ChangeClassAsync(id, model.ClassId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (homeModel.StartTime.ToString() != model.StartTime)
                {
                    var changeResponse = await ChangeStartTimeAsync(id, model.StartTime);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (homeModel.EndTime.ToString() != model.EndTime)
                {
                    var changeResponse = await ChangeEndTimeAsync(id, model.EndTime);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (model.LateSubmitDays.HasValue)
                {
                    var changeResponse = await ChangeLateSubmitDaysAsync(id, model.LateSubmitDays.Value);
                    if (!changeResponse.Success) return changeResponse;
                }

                if(homeModel.AchievedPercentage != model.Achieved_Percentage)
                {
                    var changeResponse = await ChangePercentageAsync(id, model.Achieved_Percentage);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (homeModel.Time.ToString() != model.Time)
                {
                    var changeResponse = await ChangeTimeAsync(id, model.Time);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (homeModel.Title != model.Title)
                {
                    var changeResponse = await ChangeTitleAsync(id, model.Title);
                    if (!changeResponse.Success) return changeResponse;
                }

                await _unit.CommitTransAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch(Exception ex)
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
