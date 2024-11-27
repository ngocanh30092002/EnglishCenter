using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.HomeworkTasks
{
    public class HwSubRecordService : IHwSubRecordService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public HwSubRecordService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> ChangeHomeQuesAsync(long id, long homeQueId)
        {
            var subRecord = _unit.HwSubRecords.GetById(id);
            if (subRecord == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submission records",
                    Success = false
                };
            }

            var isExistHomeQues = _unit.HomeQues.IsExist(h => h.HomeQuesId == homeQueId);
            if (!isExistHomeQues)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubRecords.ChangeHomeQuesAsync(subRecord, homeQueId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change homework question failed",
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

        public async Task<Response> ChangeSelectedAnswerAsync(long id, string selectedAnswer)
        {
            var subRecord = _unit.HwSubRecords.GetById(id);
            if (subRecord == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submission records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubRecords.ChangeSelectedAnswerAsync(subRecord, selectedAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change selected answer failed",
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

        public async Task<Response> ChangeSubAsync(long id, long? subId)
        {
            var subRecord = _unit.HwSubRecords.GetById(id);
            if (subRecord == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submission records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubRecords.ChangeSubAsync(subRecord, subId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change sub homework questions failed",
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

        public async Task<Response> ChangeSubmissionAsync(long id, long hwSubId)
        {
            var subRecord = _unit.HwSubRecords.GetById(id);
            if (subRecord == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submission records",
                    Success = false
                };
            }

            var isExistSubmission = _unit.HwSubmissions.IsExist(h => h.SubmissionId == hwSubId);
            if (!isExistSubmission)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submission",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubRecords.ChangeSubmissionAsync(subRecord, hwSubId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change submission failed",
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

        public async Task<Response> CreateAsync(HwSubRecordDto model)
        {
            var submissionModel = _unit.HwSubmissions.GetById(model.SubmissionId);
            if (submissionModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var homeQueModel = _unit.HomeQues.GetById(model.HwQuesId);
            if (homeQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework questions",
                    Success = false
                };
            }

            if (homeQueModel.HomeworkId != submissionModel.HomeworkId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Homework question must belong to same Homework",
                    Success = false
                };
            }

            var isExistSub = await _unit.HwSubRecords.IsExistSubAsync((QuesTypeEnum)homeQueModel.Type, model.HwSubQuesId);
            if (!isExistSub)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Sub Id isn't valid",
                    Success = false
                };
            }

            var isCorrectAnswer = await _unit.HomeQues.IsCorrectAnswerAsync(homeQueModel, model.SelectedAnswer, model.HwSubQuesId);

            var existRecords = _unit.HwSubRecords
                                    .Find(a => a.SubmissionId == model.SubmissionId &&
                                               a.HwQuesId == model.HwQuesId &&
                                               a.HwSubQuesId == model.HwSubQuesId)
                                    .FirstOrDefault();

            if (existRecords != null)
            {
                _unit.HwSubRecords.Remove(existRecords);
            }

            var hwSubRecord = _mapper.Map<HwSubRecord>(model);

            hwSubRecord.IsCorrect = isCorrectAnswer;
            _unit.HwSubRecords.Add(hwSubRecord);

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
            var subRecord = _unit.HwSubRecords.GetById(id);
            if (subRecord == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submission records",
                    Success = false
                };
            }

            var submissionModel = await _unit.HwSubmissions
                                             .Include(s => s.Homework)
                                             .Include(s => s.SubRecords)
                                             .FirstOrDefaultAsync(s => s.SubmissionId == subRecord.SubmissionId);
            if (submissionModel == null)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Delete failed",
                    Success = false
                };
            }

            var correctNum = submissionModel.SubRecords
                                            .Where(r => r.IsCorrect == true && r.RecordId != subRecord.RecordId)
                                            .Count();

            var totalNum = await _unit.HomeQues.GetNumberByHomeworkAsync(submissionModel.Homework.HomeworkId);
            submissionModel.IsPass = (correctNum * 100.0 / totalNum) >= submissionModel.Homework.AchievedPercentage;

            _unit.HwSubRecords.Remove(subRecord);
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
            var subModels = _unit.HwSubRecords.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HwSubRecordResDto>>(subModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var subModel = _unit.HwSubRecords.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HwSubRecordResDto>(subModel),
                Success = true
            });
        }

        public Task<Response> GetByHwSubmitAsync(long hwSubId)
        {
            var subModels = _unit.HwSubRecords.Find(h => h.SubmissionId == hwSubId).ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HwSubRecordResDto>>(subModels),
                Success = true
            });
        }

        public async Task<Response> GetResultAsync(long id)
        {
            var submissionModel = await _unit.HwSubmissions
                                       .Include(s => s.Homework)
                                       .FirstOrDefaultAsync(s => s.SubmissionId == id);

            if (submissionModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submissions",
                    Success = false
                };
            }

            if (submissionModel.SubmitStatus == (int)SubmitStatusEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't get result with this status",
                    Success = false
                };
            }

            var records = _unit.HwSubRecords
                                    .Find(r => r.SubmissionId == submissionModel.SubmissionId)
                                    .ToList();

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };


            var homeSubRecords = new List<HwSubRecordResDto>();

            foreach (var record in records)
            {
                var resultModel = _mapper.Map<HwSubRecordResDto>(record);
                var answerInfo = await _unit.HomeQues.GetAnswerInfoAsync(record.HwQuesId, record.HwSubQuesId);
                resultModel.AnswerInfo = answerInfo;

                homeSubRecords.Add(resultModel);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = homeSubRecords,
                Success = true
            };

        }

        public async Task<Response> UpdateAsync(long id, HwSubRecordDto model)
        {
            var subModel = _unit.HwSubRecords.GetById(id);
            if (subModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework question records",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();
            try
            {
                if (subModel.SubmissionId != model.SubmissionId)
                {
                    var changeResponse = await ChangeSubmissionAsync(id, model.SubmissionId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (subModel.HwQuesId != model.HwQuesId)
                {
                    var changeResponse = await ChangeHomeQuesAsync(id, model.HwQuesId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (subModel.HwSubQuesId != model.HwSubQuesId)
                {
                    var changeResponse = await ChangeSubAsync(id, model.HwSubQuesId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (subModel.SelectedAnswer != model.SelectedAnswer)
                {
                    var changeResponse = await ChangeSelectedAnswerAsync(id, model.SelectedAnswer);
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
