using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class AnswerToeicService : IAnswerToeicService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly ISubToeicService _subService;

        public AnswerToeicService(IUnitOfWork unit, IMapper mapper, ISubToeicService subService)
        {
            _unit = unit;
            _mapper = mapper;
            _subService = subService;
        }
        public async Task<Response> ChangeAnswerAAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeAnswerAAsync(answerModel, newAnswer);

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

        public async Task<Response> ChangeAnswerBAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeAnswerBAsync(answerModel, newAnswer);

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

        public async Task<Response> ChangeAnswerCAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeAnswerCAsync(answerModel, newAnswer);

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

        public async Task<Response> ChangeAnswerDAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeAnswerDAsync(answerModel, newAnswer);

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

        public async Task<Response> ChangeCorrectAnswerAsync(long id, string newCorrectAnswer)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeCorrectAnswerAsync(answerModel, newCorrectAnswer);

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

        public async Task<Response> ChangeExplanationAsync(long id, string newExplanation)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeExplanationAsync(answerModel, newExplanation);

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

        public async Task<Response> ChangeQuestionAsync(long id, string newQuestion)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerToeic.ChangeQuestionAsync(answerModel, newQuestion);

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

        public async Task<Response> CreateAsync(AnswerToeicDto model)
        {
            var answerModel = _mapper.Map<AnswerToeic>(model);

            _unit.AnswerToeic.Add(answerModel);
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
            var answerModel = _unit.AnswerToeic
                                   .Include(a => a.SubToeic)
                                   .FirstOrDefault(a => a.AnswerId == id);

            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            if (answerModel.SubToeic != null)
            {
                return await _subService.DeleteAsync(answerModel.SubToeic.SubId);
            }
            else
            {
                _unit.AnswerToeic.Remove(answerModel);
                await _unit.CompleteAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
        }

        public Task<Response> GetAllAsync()
        {
            var answerModels = _unit.AnswerToeic.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AnswerToeicDto>>(answerModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var answerModel = _unit.AnswerToeic.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AnswerToeicDto>(answerModel),
                Success = true
            });
        }

        public Task<Response> GetByToeicAsync(long toeicId)
        {
            var subToeic = _unit.SubToeic
                               .Include(s => s.QuesToeic)
                               .Include(s => s.Answer)
                               .Where(s => s.QuesToeic.ToeicId == toeicId)
                               .OrderBy(s => s.QuesNo);

            var answerResDtos = new List<AnswerToeicResDto>();

            foreach (var item in subToeic)
            {
                var answerModel = new AnswerToeicResDto();
                answerModel.QuesNo = answerModel.QuesNo;
                answerModel.Question = item.Answer?.Question;
                answerModel.AnswerA = item.Answer?.AnswerA;
                answerModel.AnswerB = item.Answer?.AnswerB;
                answerModel.AnswerC = item.Answer?.AnswerC;
                answerModel.AnswerD = item.Answer?.AnswerD;
                answerModel.Explanation = item.Answer?.Explanation;
                answerModel.CorrectAnswer = item.Answer?.CorrectAnswer;

                answerResDtos.Add(answerModel);
            }

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = answerResDtos,
                Success = true
            });

        }
        public async Task<Response> UpdateAsync(long id, AnswerToeicDto model)
        {
            var isSuccess = await _unit.AnswerToeic.UpdateAsync(id, model);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Update failed",
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
