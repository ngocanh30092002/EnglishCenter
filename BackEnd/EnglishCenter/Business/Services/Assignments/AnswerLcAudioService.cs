using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Assignments
{
    public class AnswerLcAudioService : IAnswerLcAudioService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IQuesLcAudioService _quesService;

        public AnswerLcAudioService(IUnitOfWork unit, IMapper mapper, IQuesLcAudioService quesService)
        {
            _unit = unit;
            _mapper = mapper;
            _quesService = quesService;
        }
        public async Task<Response> ChangeAnswerAAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerLcAudios.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcAudios.ChangeAnswerAAsync(answerModel, newAnswer);

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
            var answerModel = _unit.AnswerLcAudios.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcAudios.ChangeAnswerBAsync(answerModel, newAnswer);

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
            var answerModel = _unit.AnswerLcAudios.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcAudios.ChangeAnswerCAsync(answerModel, newAnswer);

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

        public async Task<Response> ChangeCorrectAnswerAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerLcAudios.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcAudios.ChangeCorrectAnswerAsync(answerModel, newAnswer);

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

        public async Task<Response> ChangeQuestionAsync(long id, string newQues)
        {
            var answerModel = _unit.AnswerLcAudios.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcAudios.ChangeQuestionAsync(answerModel, newQues);
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

        public async Task<Response> CreateAsync(AnswerLcAudioDto model)
        {
            var answerModel = _mapper.Map<AnswerLcAudio>(model);

            _unit.AnswerLcAudios.Add(answerModel);
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
            var answerModel = _unit.AnswerLcAudios
                                    .Include(a => a.QuesLcAudio)
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

            if(answerModel.QuesLcAudio != null)
            {
                return await _quesService.DeleteAsync(answerModel.QuesLcAudio.QuesId);
            }
            else
            {
                _unit.AnswerLcAudios.Remove(answerModel);
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
            var answerModels = _unit.AnswerLcAudios.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AnswerLcAudioDto>>(answerModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var answerModel = _unit.AnswerLcAudios.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AnswerLcAudioDto>(answerModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, AnswerLcAudioDto model)
        {
            var isSuccess = await _unit.AnswerLcAudios.UpdateAsync(id, model);
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
