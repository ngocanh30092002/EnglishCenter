using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class SubToeicService : ISubToeicService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public SubToeicService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> ChangeAnswerAAsync(long id, string newAnswer)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeAnswerAAsync(subModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change answerA failed",
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

        public async Task<Response> ChangeAnswerAsync(long id, long answerId)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeAnswerAsync(subModel, answerId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change answer failed",
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

        public async Task<Response> ChangeAnswerBAsync(long id, string newAnswer)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeAnswerBAsync(subModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change answerB failed",
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

        public async Task<Response> ChangeAnswerCAsync(long id, string newAnswer)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeAnswerCAsync(subModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change answerC failed",
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

        public async Task<Response> ChangeAnswerDAsync(long id, string newAnswer)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeAnswerDAsync(subModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change answerD failed",
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

        public async Task<Response> ChangeQuesNoAsync(long id, int queNo)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeQuesNoAsync(subModel, queNo);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question no failed",
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

        public async Task<Response> ChangeQuestionAsync(long id, string question)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubToeic.ChangeQuestionAsync(subModel, question);
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

        public async Task<Response> CreateAsync(SubToeicDto model)
        {
            if (!model.QuesId.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Question id must be required",
                    Success = false
                };
            }

            var queModel = _unit.QuesToeic.Include(q => q.SubToeicList).FirstOrDefault(q => q.QuesId == model.QuesId.Value);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic questions",
                    Success = false
                };
            }

            var subModels = _unit.SubToeic
                                  .Include(s => s.QuesToeic)
                                  .Where(s => s.QuesToeic.ToeicId == queModel.ToeicId);

            var isFullExam = subModels.Count() == GlobalVariable.TOEIC_NUM;

            if (isFullExam)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't add more sub question because it's full",
                    Success = false
                };
            }

            var isFullEachPart = await IsFullEachPartAsync(queModel);
            if (isFullEachPart)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = $"Part {queModel.Part} has maximum question",
                    Success = false
                };
            }

            var nextQueNo = await NextQueNoAsync(queModel.QuesId);
            if (nextQueNo == -1)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't create sub toeic question",
                    Success = false
                };
            }

            var isDuplicated = subModels.Where(s => s.QuesNo == nextQueNo).Any();
            if (isDuplicated)
            {
                var duplicatedModel = subModels.Where(s => s.QuesNo == nextQueNo).FirstOrDefault();
                if (duplicatedModel != null)
                {
                    var greaterModel = subModels.Where(s => s.QuesNo >= duplicatedModel.QuesNo &&
                                                            s.QuesToeic.Part == queModel.Part);
                    foreach (var item in greaterModel)
                    {
                        item.QuesNo = item.QuesNo + 1;
                    }
                }
            }

            var subToeicModel = _mapper.Map<SubToeic>(model);
            subToeicModel.QuesNo = nextQueNo;

            if (model.AnswerId.HasValue)
            {
                var isExistAnswer = _unit.AnswerToeic.IsExist(a => a.AnswerId == model.AnswerId.Value);
                if (!isExistAnswer)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any answers",
                        Success = false
                    };
                }

                subToeicModel.AnswerId = model.AnswerId.Value;
            }
            else
            {
                if (model.Answer != null)
                {
                    var answerModel = _mapper.Map<AnswerToeic>(model.Answer);
                    _unit.AnswerToeic.Add(answerModel);

                    subToeicModel.Answer = answerModel;
                }
            }


            _unit.SubToeic.Add(subToeicModel);
            queModel.IsGroup = queModel.SubToeicList.Count > 1;

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
            var subModel = await _unit.SubToeic
                                .Include(s => s.QuesToeic)
                                .Include(s => s.Answer)
                                .FirstOrDefaultAsync(s => s.SubId == id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            var greaterModels = _unit.SubToeic
                                     .Include(s => s.QuesToeic)
                                     .Where(s => s.QuesNo > subModel.QuesNo &&
                                                 s.QuesToeic.Part == subModel.QuesToeic.Part &&
                                                 s.QuesToeic.ToeicId == subModel.QuesToeic.ToeicId);

            foreach (var model in greaterModels)
            {
                model.QuesNo = model.QuesNo - 1;
            }

            if (subModel.Answer != null)
            {
                _unit.AnswerToeic.Remove(subModel.Answer);
            }

            _unit.SubToeic.Remove(subModel);

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
            var subModels = _unit.SubToeic.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubToeicResDto>>(subModels),
                Success = false
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var subModel = _unit.SubToeic.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<SubToeicResDto>(subModel),
                Success = false
            });
        }

        public async Task<Response> UpdateAsync(long id, SubToeicDto model)
        {
            var subModel = _unit.SubToeic.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub toeic questions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (model.QuesNo.HasValue && subModel.QuesNo != model.QuesNo)
                {
                    var response = await ChangeQuesNoAsync(id, model.QuesNo.Value);
                    if (!response.Success) return response;
                }

                if (subModel.Question != model.Question)
                {
                    var response = await ChangeQuestionAsync(id, model.Question ?? "");
                    if (!response.Success) return response;
                }

                if (subModel.AnswerA != model.AnswerA)
                {
                    var response = await ChangeAnswerAAsync(id, model.AnswerA ?? "");
                    if (!response.Success) return response;
                }

                if (subModel.AnswerB != model.AnswerB)
                {
                    var response = await ChangeAnswerBAsync(id, model.AnswerB ?? "");
                    if (!response.Success) return response;
                }

                if (subModel.AnswerC != model.AnswerC)
                {
                    var response = await ChangeAnswerCAsync(id, model.AnswerC ?? "");
                    if (!response.Success) return response;
                }

                if (subModel.AnswerD != model.AnswerD)
                {
                    var response = await ChangeAnswerDAsync(id, model.AnswerD ?? "");
                    if (!response.Success) return response;
                }

                if (model.AnswerId.HasValue && subModel.AnswerId != model.AnswerId)
                {
                    var response = await ChangeAnswerAsync(id, model.AnswerId.Value);
                    if (!response.Success) return response;
                }

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

        private async Task<bool> IsFullEachPartAsync(QuesToeic queModel)
        {
            var sameQues = new List<SubToeic>();

            switch (queModel.Part)
            {
                case (int)PartEnum.Part1:
                case (int)PartEnum.Part2:
                case (int)PartEnum.Part5:
                    sameQues = await _unit.SubToeic
                                          .Include(s => s.QuesToeic)
                                          .Where(s => s.QuesToeic.ToeicId == queModel.ToeicId && s.QuesToeic.Part == queModel.Part)
                                          .ToListAsync();
                    break;

                default:
                    sameQues = _unit.SubToeic
                                    .Find(s => s.QuesId == queModel.QuesId)
                                    .ToList();
                    break;
            }

            switch (queModel.Part)
            {
                case (int)PartEnum.Part1:
                    return sameQues.Count >= 6;
                case (int)PartEnum.Part2:
                    return sameQues.Count >= 25;
                case (int)PartEnum.Part3:
                    return sameQues.Count >= 3;
                case (int)PartEnum.Part4:
                    return sameQues.Count >= 3;
                case (int)PartEnum.Part5:
                    return sameQues.Count >= 30;
                case (int)PartEnum.Part6:
                    return sameQues.Count >= 4;
                case (int)PartEnum.Part7:
                    return sameQues.Count >= 5;
                default:
                    throw new NotImplementedException();
            }
        }

        public async Task<int> NextQueNoAsync(long quesId)
        {
            var queModel = _unit.QuesToeic.Include(q => q.SubToeicList).FirstOrDefault(q => q.QuesId == quesId);
            if (queModel == null) return -1;

            int startNum = 0;
            int endNum = 0;

            var numbers = await _unit.SubToeic
                                        .Include(s => s.QuesToeic)
                                        .Where(s => s.QuesToeic.ToeicId == queModel.ToeicId && s.QuesToeic.Part == queModel.Part)
                                        .OrderBy(s => s.QuesNo)
                                        .Select(s => s.QuesNo)
                                        .ToListAsync();

            switch (queModel.Part)
            {
                case (int)PartEnum.Part1:
                    startNum = 1;
                    endNum = 6;
                    break;
                case (int)PartEnum.Part2:
                    startNum = 7;
                    endNum = 31;
                    break;
                case (int)PartEnum.Part3:
                    startNum = 32;
                    endNum = 70;
                    break;
                case (int)PartEnum.Part4:
                    startNum = 71;
                    endNum = 100;
                    break;
                case (int)PartEnum.Part5:
                    startNum = 101;
                    endNum = 130;
                    break;
                case (int)PartEnum.Part6:
                    startNum = 131;
                    endNum = 146;
                    break;
                case (int)PartEnum.Part7:
                    startNum = 147;
                    endNum = 200;
                    break;
            }

            if (numbers.Count == 0)
            {
                switch (queModel.Part)
                {
                    case (int)PartEnum.Part1:
                    case (int)PartEnum.Part2:
                    case (int)PartEnum.Part5:
                        return startNum;
                    default:
                        var currentNum = _unit.SubToeic
                                               .Include(s => s.QuesToeic)
                                               .Where(s => s.QuesToeic.ToeicId == queModel.ToeicId && s.QuesToeic.Part == queModel.Part)
                                               .Select(s => (int?)s.QuesNo)
                                               .Max();
                        return currentNum.HasValue ? currentNum.Value + 1 : startNum;
                }
            }

            int result = -1;
            for (int i = startNum; i <= endNum; i++)
            {
                if (i - startNum >= numbers.Count)
                {
                    result = i;
                    break;
                }

                if (numbers[i - startNum] != i)
                {
                    result = i;
                    break;
                }
            }

            return result;
        }
    }
}
