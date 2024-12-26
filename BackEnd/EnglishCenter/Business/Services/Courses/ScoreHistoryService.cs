using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Courses
{
    public class ScoreHistoryService : IScoreHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;

        public ScoreHistoryService(IMapper mapper, IUnitOfWork unit)
        {
            _mapper = mapper;
            _unit = unit;
        }
        public async Task<Response> ChangeEntrancePointAsync(long scoreId, int score)
        {
            var scoreHisModel = _unit.ScoreHis.GetById(scoreId);
            if (scoreHisModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any score history"
                };
            }

            var isSuccess = await _unit.ScoreHis.ChangeEntrancePointAsync(scoreHisModel, score);
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

        public async Task<Response> ChangeFinalPointAsync(long scoreId, int score)
        {
            var scoreHisModel = _unit.ScoreHis.GetById(scoreId);
            if (scoreHisModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any score history"
                };
            }

            var isSuccess = await _unit.ScoreHis.ChangeFinalPointAsync(scoreHisModel, score);
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

        public async Task<Response> ChangeMidtermPointAsync(long scoreId, int score)
        {
            var scoreHisModel = _unit.ScoreHis.GetById(scoreId);
            if (scoreHisModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any score history"
                };
            }

            var isSuccess = await _unit.ScoreHis.ChangeMidtermPointAsync(scoreHisModel, score);
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

        public async Task<Response> CreateAsync(ScoreHistoryDto model)
        {
            if (model.EntrancePoint < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Entrance Point must be greater than 0"
                };
            }

            if (model.MidtermPoint < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Midterm Point must be greater than 0"
                };
            }

            if (model.FinalPoint < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Final Point must be greater than 0"
                };
            }

            var scoreModel = _mapper.Map<ScoreHistory>(model);

            _unit.ScoreHis.Add(scoreModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> DeleteAsync(long scoreId)
        {
            var scoreHisModel = _unit.ScoreHis.GetById(scoreId);
            if (scoreHisModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any score history"
                };
            }

            _unit.ScoreHis.Remove(scoreHisModel);
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
            var scoreHis = _unit.ScoreHis.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ScoreHistoryDto>>(scoreHis),
                Success = true
            });
        }

        public Task<Response> GetAsync(long scoreId)
        {
            var scoreHis = _unit.ScoreHis.GetById(scoreId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ScoreHistoryDto>(scoreHis),
                Success = true
            });
        }

        public async Task<Response> GetByClassAsync(string classId)
        {
            var isExistClass = _unit.Classes.IsExist(e => e.ClassId == classId);
            if (!isExistClass)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any classes",
                    Success = false
                };
            }

            var scoreHisModels = await _unit.ScoreHis.GetByClassAsync(classId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ScoreHisResDto>>(scoreHisModels),
                Success = true
            };

        }

        public async Task<Response> UpdateAsync(long scoreId, ScoreHistoryDto model)
        {
            var response = await _unit.ScoreHis.UpdateAsync(scoreId, model);
            if (response.Success)
            {
                await _unit.CompleteAsync();
            }

            return response;
        }
    }
}
