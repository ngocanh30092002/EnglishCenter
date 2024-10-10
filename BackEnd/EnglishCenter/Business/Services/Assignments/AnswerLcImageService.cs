using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Assignments
{
    public class AnswerLcImageService : IAnswerLcImageService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IQuesLcImageService _quesService;

        public AnswerLcImageService(IUnitOfWork unit, IMapper mapper, IQuesLcImageService quesService)
        {
            _unit = unit;
            _mapper = mapper;
            _quesService = quesService;
        }

        public async Task<Response> ChangeAnswerAAsync(long id, string newAnswer)
        {
            var answerModel = _unit.AnswerLcImages.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcImages.ChangeAnswerAAsync(answerModel, newAnswer);

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
            var answerModel = _unit.AnswerLcImages.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcImages.ChangeAnswerBAsync(answerModel, newAnswer);

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
            var answerModel = _unit.AnswerLcImages.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcImages.ChangeAnswerCAsync(answerModel, newAnswer);

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
            var answerModel = _unit.AnswerLcImages.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcImages.ChangeAnswerDAsync(answerModel, newAnswer);

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
            var answerModel = _unit.AnswerLcImages.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answers",
                    Success = false
                };
            }

            var isSuccess = await _unit.AnswerLcImages.ChangeCorrectAnswerAsync(answerModel, newAnswer);

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

        public async Task<Response> CreateAsync(AnswerLcImageDto model)
        {
            var answerModel = _mapper.Map<AnswerLcImage>(model);

            _unit.AnswerLcImages.Add(answerModel);
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
            var answerModel = _unit.AnswerLcImages
                                   .Include(a => a.QuesLcImage)
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

            if(answerModel.QuesLcImage != null)
            {
                return await _quesService.DeleteAsync(answerModel.QuesLcImage.QuesId);
            }
            else
            {
                _unit.AnswerLcImages.Remove(answerModel);
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
            var answerModels = _unit.AnswerLcImages.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AnswerLcImageDto>>(answerModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var answerModel = _unit.AnswerLcImages.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AnswerLcImageDto>(answerModel),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, AnswerLcImageDto model)
        {
            var isSuccess = await _unit.AnswerLcImages.UpdateAsync(id, model);
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
