using System.Text.RegularExpressions;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class ToeicPracticeRecordService : IToeicPracticeRecordService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public ToeicPracticeRecordService(IMapper mapper, IUnitOfWork unit, UserManager<User> userManager)
        {
            _unit = unit;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Response> ChangeSelectedAnswerAsync(long id, string? selectedAnswer)
        {
            var recordModel = _unit.ToeicPracticeRecords
                                   .Include(r => r.SubToeic)
                                   .FirstOrDefault(r => r.RecordId == id);

            if (recordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicPracticeRecords.ChangeSelectedAnswerAsync(recordModel, selectedAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change selected answer",
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

        public async Task<Response> CreateAsync(ToeicPracticeRecordDto model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users",
                    Success = false
                };
            }

            var subQues = _unit.SubToeic
                               .Include(s => s.Answer)
                               .Include(s => s.QuesToeic)
                               .FirstOrDefault(s => s.SubId == model.SubId);

            if (subQues == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub questions",
                    Success = false
                };
            }

            var attemptModel = _unit.ToeicAttempts.Include(a => a.ToeicExam).FirstOrDefault(a => a.AttemptId == model.AttemptId);

            if (attemptModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any attempt models",
                    Success = false
                };
            }

            if (attemptModel.ToeicExam.ToeicId != subQues.QuesToeic.ToeicId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Question isn't belong to toeic exam",
                    Success = false
                };
            }

            if (!string.IsNullOrEmpty(model.SelectedAnswer))
            {
                string pattern = "^[ABCDabcd]+$";
                if (!Regex.IsMatch(model.SelectedAnswer, pattern))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Selected Answer isn't valid",
                        Success = false
                    };
                }
            }

            var toeicModel = new ToeicPracticeRecord()
            {
                AttemptId = model.AttemptId,
                SubQueId = model.SubId,
                SelectedAnswer = model.SelectedAnswer,
            };

            toeicModel.IsCorrect = subQues.Answer == null ? false : subQues.Answer.CorrectAnswer == model.SelectedAnswer;

            _unit.ToeicPracticeRecords.Add(toeicModel);
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
            var toeicModel = _unit.ToeicPracticeRecords.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic records",
                    Success = false
                };
            }

            _unit.ToeicPracticeRecords.Remove(toeicModel);

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, ToeicPracticeRecordDto model)
        {
            var toeicModel = _unit.ToeicPracticeRecords.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic records",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (toeicModel.SelectedAnswer != model.SelectedAnswer)
                {
                    var response = await ChangeSelectedAnswerAsync(id, model.SelectedAnswer);
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

        public Task<Response> GetAllAsync()
        {
            var toeicModels = _unit.ToeicPracticeRecords.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicPracticeRecordResDto>>(toeicModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var toeicModels = _unit.ToeicPracticeRecords.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicPracticeRecordResDto>(toeicModels),
                Success = true
            });
        }

        public async Task<Response> GetResultAsync(long attemptId, string userId)
        {
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

            if (attemptModel.UserId != userId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Message = "You are not allowed to access other people's records.",
                    Success = false
                };
            }

            var records = await _unit.ToeicPracticeRecords.GetResultAsync(attemptId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicPracticeRecordResDto>>(records),
                Success = true
            };
        }

        public async Task<Response> GetResultScoreAsync(long attemptId, string userId)
        {
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

            if (attemptModel.UserId != userId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Message = "You are not allowed to access other people's records.",
                    Success = false
                };
            }

            var res = new ToeicScoreResDto()
            {
                Part1 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part1),
                Part2 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part2),
                Part3 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part3),
                Part4 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part4),
                Part5 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part5),
                Part6 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part6),
                Part7 = await _unit.ToeicPracticeRecords.GetNumCorrectRecordsWithPartAsync(attemptId, (int)PartEnum.Part7),
            };

            var listeningCon = await _unit.ToeicConversion.GetByNumberCorrectAsync(res.Part1 + res.Part2 + res.Part3 + res.Part4, ToeicEnum.Listening);
            var readingCon = await _unit.ToeicConversion.GetByNumberCorrectAsync(res.Part5 + res.Part6 + res.Part7, ToeicEnum.Reading);

            res.Listening = listeningCon == null ? 0 : listeningCon.EstimatedScore;
            res.Reading = readingCon == null ? 0 : readingCon.EstimatedScore;

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = res,
                Success = true
            };
        }


    }
}


