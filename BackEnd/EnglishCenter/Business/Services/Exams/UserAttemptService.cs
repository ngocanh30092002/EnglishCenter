using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class UserAttemptService : IUserAttemptService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IAttemptRecordService _attemptRecordsService;
        private readonly UserManager<User> _userManager;

        public UserAttemptService(IMapper mapper, IUnitOfWork unit, UserManager<User> userManager, IAttemptRecordService attemptRecordService)
        {
            _mapper = mapper;
            _unit = unit;
            _attemptRecordsService = attemptRecordService;
            _userManager = userManager;
        }

        public async Task<Response> ChangeListeningScoreAsync(long id, int score)
        {
            var attemptModel = _unit.UserAttempts.GetById(id);
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

            var isChangeSuccess = await _unit.UserAttempts.ChangeListeningScoreAsync(attemptModel, score);
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
            var attemptModel = _unit.UserAttempts.GetById(id);
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

            var isChangeSuccess = await _unit.UserAttempts.ChangeReadingScoreAsync(attemptModel, score);
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

        public async Task<Response> ChangeRoadMapExamAsync(long id, long roadMapExamId)
        {
            var attemptModel = _unit.UserAttempts.GetById(id);
            if (attemptModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            var isExistToeic = _unit.ToeicExams.IsExist(e => e.ToeicId == roadMapExamId);
            if (!isExistToeic)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic examinations",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.UserAttempts.ChangeRoadMapExamAsync(attemptModel, roadMapExamId);
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

        public async Task<Response> ChangeToeicAsync(long id, long toeicId)
        {
            var attemptModel = _unit.UserAttempts.GetById(id);
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

            var isChangeSuccess = await _unit.UserAttempts.ChangeToeicAsync(attemptModel, toeicId);
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
            var attemptModel = _unit.UserAttempts.GetById(id);
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

            var isChangeSuccess = await _unit.UserAttempts.ChangeUserAsync(attemptModel, userId);
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

        public async Task<Response> CreateAsync(UserAttemptDto model)
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

            if (!(model.ToeicId.HasValue ^ model.RoadMapExamId.HasValue))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Attempt only belong to toeic or road map exams",
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

            if (model.ToeicId.HasValue)
            {
                var toeicModel = _unit.ToeicExams.GetById(model.ToeicId.Value);
                if (toeicModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any toeic examinations",
                        Success = false
                    };
                }

                var previousModel = _unit.UserAttempts
                                   .Find(a => a.UserId == model.UserId &&
                                              a.ToeicId == model.ToeicId.Value &&
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
            }

            if (model.RoadMapExamId.HasValue)
            {
                var roadMapModel = _unit.RoadMapExams.GetById(model.RoadMapExamId.Value);
                if (roadMapModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any road map exams",
                        Success = false
                    };
                }

                var previousModel = _unit.UserAttempts
                                 .Find(a => a.UserId == model.UserId &&
                                            a.RoadMapExamId == model.RoadMapExamId.Value &&
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
            }

            var attemptModel = new UserAttempt()
            {
                ToeicId = model.ToeicId,
                RoadMapExamId = model.RoadMapExamId,
                UserId = userModel.Id,
                ListeningScore = model.Listening_Score,
                ReadingScore = model.Reading_Score,
                Date = DateTime.Now,
                IsSubmitted = false
            };

            _unit.UserAttempts.Add(attemptModel);

            if (model.ToeicId.HasValue)
            {
                var toeicModel = _unit.ToeicExams.GetById(model.ToeicId.Value);
                toeicModel.CompletedNum++;
            }
            else
            {
                var roadMapExam = _unit.RoadMapExams.GetById(model.RoadMapExamId!.Value);
                roadMapExam.CompletedNum++;
            }

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
            var models = _unit.UserAttempts.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserAttemptResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.UserAttempts.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<UserAttemptResDto>(model),
                Success = true
            });
        }

        public Task<Response> GetByCourseAsync(string userId, string courseId)
        {
            var models = _unit.UserAttempts
                            .Include(a => a.ToeicExam)
                            .Include(a => a.RoadMapExam)
                            .ThenInclude(a => a.RoadMap)
                            .Where(a => a.UserId == userId && a.RoadMapExamId.HasValue && a.RoadMapExam.RoadMap.CourseId == courseId)
                            .ToList();


            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserAttemptResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetByToeicAsync(string userId)
        {
            var models = _unit.UserAttempts
                             .Include(a => a.ToeicExam)
                             .Include(a => a.RoadMapExam)
                             .Where(a => a.UserId == userId && a.ToeicId.HasValue)
                             .ToList();


            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserAttemptResDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetByUserAsync(string userId)
        {
            var models = _unit.UserAttempts
                             .Include(a => a.ToeicExam)
                             .Include(a => a.RoadMapExam)
                             .Where(a => a.UserId == userId)
                             .ToList();


            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<UserAttemptResDto>>(models),
                Success = true
            });
        }

        public async Task<Response> HandleSubmitToeicAsync(long attemptId, UserAttemptDto model)
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

            var attemptModel = _unit.UserAttempts.GetById(attemptId);
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

                    var res = await _attemptRecordsService.CreateAsync(item);
                    if (!res.Success) return res;
                }

                var resultRes = await _attemptRecordsService.GetResultScoreAsync(attemptModel.AttemptId, model.UserId);
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

        public async Task<Response> UpdateAsync(long id, UserAttemptDto model)
        {
            var attemptModel = _unit.UserAttempts.GetById(id);
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

                if (model.ToeicId.HasValue && model.ToeicId != attemptModel.ToeicId)
                {
                    var changeRes = await ChangeToeicAsync(id, model.ToeicId.Value);
                    if (!changeRes.Success) return changeRes;
                }

                if (model.RoadMapExamId.HasValue && model.RoadMapExamId != attemptModel.RoadMapExamId)
                {
                    var changeRes = await ChangeRoadMapExamAsync(id, model.RoadMapExamId.Value);
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
