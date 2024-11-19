using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;

namespace EnglishCenter.Business.Services.Exams
{
    public class ToeicAttemptService : IToeicAttemptService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IToeicPracticeRecordService _toeicPracticeService;
        private readonly UserManager<User> _userManager;

        public ToeicAttemptService(IMapper mapper, IUnitOfWork unit, UserManager<User> userManager, IToeicPracticeRecordService toeicPracticeService)
        {
            _mapper = mapper;
            _unit = unit;
            _toeicPracticeService = toeicPracticeService;
            _userManager = userManager;
        }

        public async Task<Response> ChangeListeningScoreAsync(long id, int score)
        {
            var attemptModel = _unit.ToeicAttempts.GetById(id);
            if (attemptModel == null)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            if (score < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Score must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicAttempts.ChangeListeningScoreAsync(attemptModel, score);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change listening score",
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

        public async Task<Response> ChangeReadingScoreAsync(long id, int score)
        {
            var attemptModel = _unit.ToeicAttempts.GetById(id);
            if (attemptModel == null)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            if (score < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Score must be greater than 0",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicAttempts.ChangeReadingScoreAsync(attemptModel, score);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change reading score",
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

        public async Task<Response> ChangeToeicAsync(long id, long toeicId)
        {
            var attemptModel = _unit.ToeicAttempts.GetById(id);
            if (attemptModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            var isExistToeic = _unit.ToeicExams.IsExist(e => e.ToeicId == toeicId);
            if (!isExistToeic)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examinations",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicAttempts.ChangeToeicAsync(attemptModel, toeicId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change toeic examinations",
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

        public async Task<Response> ChangeUserAsync(long id, string userId)
        {
            var attemptModel = _unit.ToeicAttempts.GetById(id);
            if (attemptModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            var userModel = await _userManager.FindByIdAsync(userId);
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicAttempts.ChangeUserAsync(attemptModel, userId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change user",
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

        public async Task<Response> CreateAsync(ToeicAttemptDto model)
        {
            var userModel = await _userManager.FindByIdAsync(model.UserId ?? "");
            if (userModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var toeicModel = _unit.ToeicExams.GetById(model.ToeicId);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examinations",
                    Success = false
                };
            }

            if (model.Listening_Score.HasValue && model.Listening_Score.Value < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Score must be greater than 0",
                    Success = false
                };
            }

            if (model.Reading_Score.HasValue && model.Reading_Score.Value < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Score must be greater than 0",
                    Success = false
                };
            }

            var previousModel = _unit.ToeicAttempts
                                     .Find(a => a.UserId == model.UserId &&
                                                a.ToeicId == model.ToeicId &&
                                                a.IsSubmitted == false)
                                     .FirstOrDefault();

            if (previousModel != null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = previousModel.AttemptId,
                    Success = true
                };
            }

            var attemptModel = new ToeicAttempt()
            {
                ToeicId = toeicModel.ToeicId,
                UserId = userModel.Id,
                ListeningScore = model.Listening_Score,
                ReadingScore = model.Reading_Score,
                Date = DateTime.Now,
                IsSubmitted = false
            };

            _unit.ToeicAttempts.Add(attemptModel);
            toeicModel.CompletedNum++;

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = attemptModel.AttemptId,
                Success = true
            };
        }

        public Task<Response> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Response> GetAllAsync()
        {
            var models = _unit.ToeicAttempts.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicAttemptResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.ToeicAttempts.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicAttemptResDto>(model),
                Success = true
            });
        }

        public Task<Response> GetByUserAsync(string userId)
        {
            var models = _unit.ToeicAttempts
                             .Include(a => a.ToeicExam)
                             .Where(a => a.UserId == userId)
                             .ToList();


            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicAttemptResDto>>(models),
                Success = true
            });
        }

        public async Task<Response> HandleSubmitToeicAsync(long attemptId, ToeicAttemptDto model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "User id must be required",
                    Success = false
                };
            }

            var attemptModel = _unit.ToeicAttempts.GetById(attemptId);
            if (attemptModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                foreach (var item in model.PracticeRecords)
                {
                    item.AttemptId = attemptModel.AttemptId;
                    item.UserId = model.UserId;

                    var res = await _toeicPracticeService.CreateAsync(item);
                    if (!res.Success) return res;
                }

                var resultRes = await _toeicPracticeService.GetResultScoreAsync(attemptModel.AttemptId, model.UserId);
                if (!resultRes.Success) return resultRes;

                var scoreModel = (ToeicScoreResDto)resultRes.Message!;

                attemptModel.ListeningScore = scoreModel.Listening;
                attemptModel.ReadingScore = scoreModel.Reading;
                attemptModel.IsSubmitted = true;

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

        public async Task<Response> UpdateAsync(long id, ToeicAttemptDto model)
        {
            var attemptModel = _unit.ToeicAttempts.GetById(id);
            if (attemptModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (model.UserId != attemptModel.UserId)
                {
                    var changeRes = await ChangeUserAsync(id, model.UserId ?? "");
                    if (!changeRes.Success) return changeRes;
                }

                if (model.ToeicId != attemptModel.ToeicId)
                {
                    var changeRes = await ChangeToeicAsync(id, model.ToeicId);
                    if (!changeRes.Success) return changeRes;
                }

                if (model.Listening_Score.HasValue && model.Listening_Score != attemptModel.ListeningScore)
                {
                    var changeRes = await ChangeListeningScoreAsync(id, model.Listening_Score.Value);
                    if (!changeRes.Success) return changeRes;
                }

                if (model.Reading_Score.HasValue && model.Reading_Score != attemptModel.ReadingScore)
                {
                    var changeRes = await ChangeReadingScoreAsync(id, model.Reading_Score.Value);
                    if (!changeRes.Success) return changeRes;
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
