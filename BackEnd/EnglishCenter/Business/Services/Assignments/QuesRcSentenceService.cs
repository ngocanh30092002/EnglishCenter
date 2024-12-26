using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Assignments
{
    public class QuesRcSentenceService : IQuesRcSentenceService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public QuesRcSentenceService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> ChangeAnswerAAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeAnswerAAsync(queModel, newAnswer);
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

        public async Task<Response> ChangeAnswerAsync(long quesId, long answerId)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isExistAnswer = _unit.AnswerRcSentences.IsExist(a => a.AnswerId == answerId);
            if (!isExistAnswer)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeAnswerAsync(queModel, answerId);
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

        public async Task<Response> ChangeAnswerBAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeAnswerBAsync(queModel, newAnswer);
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

        public async Task<Response> ChangeAnswerCAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeAnswerCAsync(queModel, newAnswer);
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

        public async Task<Response> ChangeAnswerDAsync(long quesId, string newAnswer)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeAnswerDAsync(queModel, newAnswer);
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

        public async Task<Response> ChangeLevelAsync(long quesId, int level)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeLevelAsync(queModel, level);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change level",
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

        public async Task<Response> ChangeQuestionAsync(long quesId, string newQuestion)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeQuestionAsync(queModel, newQuestion);
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

        public async Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);

            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.QuesRcSentences.ChangeTimeAsync(queModel, timeOnly);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change time",
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

        public async Task<Response> CreateAsync(QuesRcSentenceDto queModel)
        {
            var queEntity = _mapper.Map<QuesRcSentence>(queModel);

            if (queModel.AnswerId.HasValue)
            {
                var answerModel = await _unit.AnswerRcSentences
                                           .Include(a => a.QuesRcSentence)
                                           .FirstOrDefaultAsync(a => a.AnswerId == queModel.AnswerId);
                if (answerModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any answers",
                        Success = false
                    };
                }

                if (answerModel.QuesRcSentence != null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "This answer is from another question",
                        Success = false
                    };
                }


                queEntity.AnswerId = queModel.AnswerId;
            }

            if (!queModel.AnswerId.HasValue && queModel.Answer != null)
            {
                var answerModel = _mapper.Map<AnswerRcSentence>(queModel.Answer);
                _unit.AnswerRcSentences.Add(answerModel);

                queEntity.Answer = answerModel;
            }

            if (string.IsNullOrEmpty(queModel.Time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is required"
                };
            }
            if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }

            queEntity.Time = timeOnly;
            queEntity.Level = queModel.Level ?? 1;
            _unit.QuesRcSentences.Add(queEntity);

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long quesId)
        {
            var queModel = _unit.QuesRcSentences.GetById(quesId);
            if (queModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            if (queModel.AnswerId != null)
            {
                var answerModel = _unit.AnswerRcSentences.GetById((long)queModel.AnswerId);
                _unit.AnswerRcSentences.Remove(answerModel);
            }

            _unit.QuesRcSentences.Remove(queModel);

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
            var queModels = await _unit.QuesRcSentences
                                .Include(q => q.Answer)
                                .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcSentenceResDto>>(queModels),
                Success = true
            };
        }

        public async Task<Response> GetAsync(long quesId)
        {
            var queModel = await _unit.QuesRcSentences
                                    .Include(q => q.Answer)
                                    .FirstOrDefaultAsync(q => q.QuesId == quesId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<QuesRcSentenceResDto>(queModel),
                Success = true
            };
        }

        public async Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId)
        {
            var assignQues = _unit.AssignQues
                                  .Find(a => a.AssignmentId == assignmentId && a.Type == (int)QuesTypeEnum.Sentence)
                                  .Select(a => a.SentenceQuesId)
                                  .ToList();

            var queModels = await _unit.QuesRcSentences
                                .Include(q => q.Answer)
                                .Where(q => !assignQues.Contains(q.QuesId))
                                .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcSentenceResDto>>(queModels),
                Success = true
            };
        }

        public async Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId)
        {
            var homeQues = _unit.HomeQues
                                  .Find(a => a.HomeworkId == homeworkId && a.Type == (int)QuesTypeEnum.Sentence)
                                  .Select(a => a.SentenceQuesId)
                                  .ToList();

            var queModels = await _unit.QuesRcSentences
                                .Include(q => q.Answer)
                                .Where(q => !homeQues.Contains(q.QuesId))
                                .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesRcSentenceResDto>>(queModels),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long quesId, QuesRcSentenceDto queModel)
        {
            var queEntity = _unit.QuesRcSentences.GetById(quesId);
            if (queEntity == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            if (string.IsNullOrEmpty(queModel.Time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is required"
                };
            }
            if (!TimeOnly.TryParse(queModel.Time, out TimeOnly timeOnly))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is not in correct format"
                };
            }


            await _unit.BeginTransAsync();

            try
            {

                if (queEntity.Question != queModel.Question)
                {
                    var response = await ChangeQuestionAsync(quesId, queModel.Question);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerA != queModel.AnswerA)
                {
                    var response = await ChangeAnswerAAsync(quesId, queModel.AnswerA);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerB != queModel.AnswerB)
                {
                    var response = await ChangeAnswerBAsync(quesId, queModel.AnswerB);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerC != queModel.AnswerC)
                {
                    var response = await ChangeAnswerCAsync(quesId, queModel.AnswerC);
                    if (!response.Success) return response;
                }

                if (queEntity.AnswerD != queModel.AnswerD)
                {
                    var response = await ChangeAnswerDAsync(quesId, queModel.AnswerD);
                    if (!response.Success) return response;
                }

                if (queModel.AnswerId.HasValue)
                {
                    var response = await ChangeAnswerAsync(quesId, (long)queModel.AnswerId);
                    if (!response.Success) return response;
                }

                if (queModel.Level.HasValue && queEntity.Level != queModel.Level.Value)
                {
                    var response = await ChangeLevelAsync(quesId, queModel.Level.Value);
                    if (!response.Success) return response;
                }

                var changeRes = await ChangeTimeAsync(quesId, timeOnly);
                if (!changeRes.Success) return changeRes;

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
