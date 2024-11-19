using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Assignments
{
    public class SubRcDoubleService : ISubRcDoubleService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public SubRcDoubleService(IUnitOfWork unit, IMapper mapper) 
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<Response> ChangeAnswerAAsync(long subId, string newAnswer)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeAnswerAAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answer A",
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

        public async Task<Response> ChangeAnswerAsync(long subId, long answerId)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isExistAnswer = _unit.AnswerRcDoubles.IsExist(a => a.AnswerId == answerId);
            if (!isExistAnswer)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeAnswerAsync(queModel, answerId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answer",
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

        public async Task<Response> ChangeAnswerBAsync(long subId, string newAnswer)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeAnswerBAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answer B",
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

        public async Task<Response> ChangeAnswerCAsync(long subId, string newAnswer)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeAnswerCAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answer C",
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

        public async Task<Response> ChangeAnswerDAsync(long subId, string newAnswer)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeAnswerDAsync(queModel, newAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change answer D",
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

        public async Task<Response> ChangeNoNumAsync(long subId, int noNum)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeNoNumAsync(queModel, noNum);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change No num",
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

        public async Task<Response> ChangePreQuesAsync(long subId, long preId)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangePreQuesAsync(queModel, preId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change pre question",
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

        public async Task<Response> ChangeQuestionAsync(long subId, string newQuestion)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.SubRcDoubles.ChangeQuestionAsync(queModel, newQuestion);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change questions",
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

        public async Task<Response> CreateAsync(SubRcDoubleDto subModel)
        {
            var preQueModel = await _unit.QuesRcDoubles
                                   .Include(q => q.SubRcDoubles)
                                   .FirstOrDefaultAsync(q => q.QuesId == subModel.PreQuesId);

            if (preQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any pre questions",
                    Success = false
                };
            }

            if (preQueModel.SubRcDoubles.Count >= preQueModel.Quantity)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't add more sub questions",
                    Success = false
                };
            }

            var subQueEntity = _mapper.Map<SubRcDouble>(subModel);

            if (subModel.AnswerId.HasValue)
            {
                var answerModel = await _unit.AnswerRcDoubles
                                            .Include(a => a.SubRcDouble)
                                            .FirstOrDefaultAsync(a => a.AnswerId == subModel.AnswerId);
                if (answerModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any answers",
                        Success = false
                    };
                }

                if (answerModel.SubRcDouble != null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "This answer is from another question",
                        Success = false
                    };
                }

                subQueEntity.AnswerId = subModel.AnswerId;
            }
            else
            {
                if (subModel.Answer != null)
                {
                    var answerModel = _mapper.Map<AnswerRcDouble>(subModel.Answer);
                    _unit.AnswerRcDoubles.Add(answerModel);

                    subQueEntity.Answer = answerModel;
                }
            }

            var currentMaxNum = preQueModel.SubRcDoubles.Count > 0 ? preQueModel.SubRcDoubles.Max(a => a.NoNum) : 0;
            subQueEntity.NoNum = currentMaxNum + 1;

            _unit.SubRcDoubles.Add(subQueEntity);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long subId)
        {
            var queModel = _unit.SubRcDoubles.GetById(subId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var currentMaxNum = _unit.SubRcDoubles
                                       .Find(s => s.PreQuesId == queModel.PreQuesId)
                                       .Select(s => (int?)s.NoNum)
                                       .Max();

            var isChangeNoNumSuccess = await _unit.SubRcDoubles.ChangeNoNumAsync(queModel, currentMaxNum ?? 1);
            if (!isChangeNoNumSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't delete questions",
                    Success = false
                };
            }

            if (queModel.AnswerId.HasValue)
            {
                var answerModel = _unit.AnswerRcDoubles.GetById(queModel.AnswerId.Value);
                _unit.AnswerRcDoubles.Remove(answerModel);
            }

            _unit.SubRcDoubles.Remove(queModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetAllAsync()
        {
            var queModels = await _unit.SubRcDoubles
                                .Include(q => q.Answer)
                                .OrderBy(q => q.NoNum)
                                .ToListAsync();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<SubRcDoubleResDto>>(queModels),
                Success = true
            };
        }

        public async Task<Response> GetAsync(long subId)
        {
            var queModel = await _unit.SubRcDoubles
                                    .Include(q => q.Answer)
                                    .FirstOrDefaultAsync(q => q.SubId == subId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<SubRcDoubleResDto>(queModel),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long subId, SubRcDoubleDto subModel)
        {
            var queEntity = _unit.SubRcDoubles.GetById(subId);
            if (queEntity == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (queEntity.PreQuesId != subModel.PreQuesId)
                {
                    var changeResponse = await ChangePreQuesAsync(subId, subModel.PreQuesId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queEntity.Question != subModel.Question)
                {
                    var changeResponse = await ChangeQuestionAsync(subId, subModel.Question);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queEntity.AnswerA != subModel.AnswerA)
                {
                    var changeResponse = await ChangeAnswerAAsync(subId, subModel.AnswerA);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queEntity.AnswerB != subModel.AnswerB)
                {
                    var changeResponse = await ChangeAnswerBAsync(subId, subModel.AnswerB);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queEntity.AnswerC != subModel.AnswerC)
                {
                    var changeResponse = await ChangeAnswerCAsync(subId, subModel.AnswerC);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (queEntity.AnswerD != subModel.AnswerD)
                {
                    var changeResponse = await ChangeAnswerDAsync(subId, subModel.AnswerD);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (subModel.NoNum.HasValue && queEntity.NoNum != subModel.NoNum)
                {
                    var changeResponse = await ChangeNoNumAsync(subId, subModel.NoNum.Value);
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
