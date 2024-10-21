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
    public class HomeQuesService : IHomeQuesService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IHwSubRecordService _subRecordService;

        public HomeQuesService(IUnitOfWork unit, IMapper mapper, IHwSubRecordService subRecordService)
        {
            _unit = unit;
            _mapper = mapper;
            _subRecordService = subRecordService;
        }

        public async Task<Response> ChangeHomeworkIdAsync(long id, long homeworkId)
        {
            var homeQueModel = _unit.HomeQues.GetById(id);
            if(homeQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework questions",
                    Success = false
                };
            }

            var isExistHomework = _unit.Homework.IsExist(h => h.HomeworkId == homeworkId);
            if (!isExistHomework)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HomeQues.ChangeHomeworkIdAsync(homeQueModel, homeworkId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change homework failed",
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

        public async Task<Response> ChangeNoNumAsync(long id, int noNum)
        {
            var homeQueModel = _unit.HomeQues.GetById(id);
            if (homeQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HomeQues.ChangeNoNumAsync(homeQueModel, noNum);

            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change no number failed",
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

        public async Task<Response> ChangeQuesAsync(long id, int type, long quesId)
        {
            var homeQueModel = _unit.HomeQues.GetById(id);
            if(homeQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework questions",
                    Success = false
                };
            }

            if(!Enum.IsDefined(typeof(QuesTypeEnum), type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid type",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HomeQues.ChangeQuesAsync(homeQueModel, (QuesTypeEnum) type, quesId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question failed",
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

        public async Task<Response> CreateAsync(HomeQueDto model)
        {
            var homeworkModel = _unit.Homework
                                    .Include(a => a.HomeQues)
                                    .FirstOrDefault(a => a.HomeworkId == model.HomeworkId);

            if(homeworkModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(QuesTypeEnum), model.Type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid type",
                    Success = false
                };
            }
            var isSameQues = await _unit.HomeQues.IsSameHomeQuesAsync((QuesTypeEnum)model.Type, model.HomeworkId, model.QuesId);
            if (isSameQues)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "It is not possible to add the same homework question",
                    Success = false
                };
            }

            var isExistQues = await _unit.HomeQues.IsExistQuesIdAsync((QuesTypeEnum)model.Type, model.QuesId);
            if (!isExistQues)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var homeQueModel = _mapper.Map<HomeQue>(model);
            var currentMaxNum = homeworkModel.HomeQues.Count > 0 ? homeworkModel.HomeQues.Max(c => c.NoNum) : 0;
            
            homeQueModel.NoNum = currentMaxNum + 1;

            _unit.HomeQues.Add(homeQueModel);

            var expectedTime = await _unit.HomeQues.GetTimeQuesAsync(homeQueModel);
            homeworkModel.ExpectedTime = homeworkModel.ExpectedTime.Add(expectedTime.ToTimeSpan());

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
            var homeQueModel = _unit.HomeQues
                                        .Include(a => a.Homework)
                                        .Include(a => a.SubRecords)
                                        .FirstOrDefault(a => a.HomeQuesId == id);

            if (homeQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework questions",
                    Success = false
                };
            }


            await _unit.BeginTransAsync();
            try
            {
                var expectedTime = await _unit.HomeQues.GetTimeQuesAsync(homeQueModel);
                homeQueModel.Homework.ExpectedTime = TimeOnly.FromTimeSpan(homeQueModel.Homework.ExpectedTime - expectedTime);

                var submitRecordIds = homeQueModel.SubRecords.Select(s => s.RecordId).ToList();
                foreach (var recordId in submitRecordIds)
                {
                    var deleteRes = await _subRecordService.DeleteAsync(recordId);
                    if (!deleteRes.Success) return deleteRes;
                }

                int i = 1;
                var otherHomeQues = _unit.HomeQues.Find(a => a.HomeworkId == homeQueModel.HomeworkId && a.NoNum != homeQueModel.NoNum);
                foreach (var assignQue in otherHomeQues)
                {
                    assignQue.NoNum = i++;
                }

                _unit.HomeQues.Remove(homeQueModel);

                await _unit.CompleteAsync();
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
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = ex.Message,
                    Success = true
                };
            }
        }

        public async Task<Response> GetAllAsync()
        {
            var models = _unit.HomeQues.GetAll();

            foreach(var model in models)
            {
                var isSuccess = await _unit.HomeQues.LoadQuestionAsync(model);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Load questions fail",
                        Success = false
                    };
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HomeQueResDto>>(models.OrderBy(m => m.NoNum)),
                Success = true
            };
        }

        public async Task<Response> GetAsync(long id)
        {
            var model = _unit.HomeQues.GetById(id);

            await _unit.HomeQues.LoadQuestionAsync(model);
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HomeQueResDto>(model),
                Success = true
            };
        }

        public async Task<Response> GetByHomeworkAsync(long homeworkId)
        {
            var models = _unit.HomeQues.Find(h => h.HomeworkId == homeworkId);
            
            foreach(var model in models)
            {
                var isSuccess = await _unit.HomeQues.LoadQuestionAsync(model);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Load questions fail",
                        Success = false
                    };
                }
            }


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HomeQueResDto>>(models.OrderBy(m => m.NoNum)),
                Success = true
            };
        }

        public async Task<Response> GetHomeQuesByNoNumAsync(long homeworkId, int noNum)
        {
            var model = _unit.HomeQues.Find(h => h.HomeworkId == homeworkId && h.NoNum == noNum).FirstOrDefault();

            if (model == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = null,
                    Success = true
                };
            }

            var isDone = await _unit.HomeQues.LoadQuestionWithAnswerAsync(model);
            if (!isDone)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HomeQueResDto>(model),
                Success = true
            };
        }

        public async Task<Response> GetNumberByHomeworkAsync(long homeworkId)
        {
            var number = await _unit.HomeQues.GetNumberByHomeworkAsync(homeworkId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = number,
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, HomeQueDto model)
        {
            var isSuccess = await _unit.HomeQues.UpdateAsync(id, model);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
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
    }
}
