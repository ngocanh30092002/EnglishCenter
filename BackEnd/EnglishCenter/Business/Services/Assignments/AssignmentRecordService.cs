using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Assignments
{
    public class AssignmentRecordService : IAssignmentRecordService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public AssignmentRecordService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;

        }

        public async Task<Response> ChangeAssignQuesAsync(long id, long assignQueId)
        {
            var answerModel = _unit.AssignmentRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isExistAssignQue = _unit.AssignQues.IsExist(x => x.AssignQuesId == assignQueId);
            if (!isExistAssignQue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignment questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignmentRecords.ChangeAssignQuesAsync(answerModel, assignQueId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change assignment question failed",
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

        public async Task<Response> ChangeProcessAsync(long id, long processId)
        {
            var answerModel = _unit.AssignmentRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignmentRecords.ChangeProcessAsync(answerModel, processId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change process failed",
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
            var answerModel = _unit.AssignmentRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignmentRecords.ChangeSelectedAnswerAsync(answerModel, selectedAnswer);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change process failed",
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
            var answerModel = _unit.AssignmentRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignmentRecords.ChangeSubAsync(answerModel, subId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change process failed",
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

        public async Task<Response> CreateAsync(AssignRecordDto model)
        {
            var processModel = _unit.LearningProcesses.GetById(model.ProcessId);
            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            var assignQueModel = _unit.AssignQues.GetById(model.AssignQuesId);
            if (assignQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignment questions",
                    Success = false
                };
            }

            if (assignQueModel.AssignmentId != processModel.AssignmentId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "AssignQues must belong to Assignment",
                    Success = false
                };
            }

            var isCorrectAnswer = false;

            if (!string.IsNullOrEmpty(model.SelectedAnswer))
            {
                var pattern = "^[ABCDabcd]+$";
                if (!Regex.IsMatch(model.SelectedAnswer, pattern))
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Selected Answer isn't valid",
                        Success = false
                    };
                }

                isCorrectAnswer = await _unit.AssignQues.IsCorrectAnswerAsync(assignQueModel, model.SelectedAnswer, model.SubId);

            }


            var isExistSub = await _unit.AssignmentRecords.IsExistSubAsync((QuesTypeEnum)assignQueModel.Type, model.SubId);
            if (!isExistSub)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Sub Id isn't valid",
                    Success = false
                };
            }


            var existAssignmentRecord = _unit.AssignmentRecords
                                        .Find(a => a.LearningProcessId == model.ProcessId &&
                                                    a.AssignQuesId == model.AssignQuesId &&
                                                    a.SubQueId == model.SubId)
                                        .FirstOrDefault();

            if (existAssignmentRecord != null)
            {
                _unit.AssignmentRecords.Remove(existAssignmentRecord);
            }

            var answerEntity = _mapper.Map<AssignmentRecord>(model);

            answerEntity.IsCorrect = isCorrectAnswer;
            _unit.AssignmentRecords.Add(answerEntity);

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
            var answerModel = _unit.AssignmentRecords.GetById(id);

            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            _unit.AssignmentRecords.Remove(answerModel);
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
            var answerModels = _unit.AssignmentRecords.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignmentRecordResDto>>(answerModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var answerModel = _unit.AssignmentRecords.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignmentRecordResDto>(answerModel),
                Success = true
            });
        }

        public Task<Response> GetByProcessIdAsync(long processId)
        {
            var answerModels = _unit.AssignmentRecords.Find(a => a.LearningProcessId == processId).ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignmentRecordResDto>>(answerModels),
                Success = true
            });
        }

        public async Task<Response> GetResultAsync(long processId)
        {
            var processModel = _unit.LearningProcesses
                                      .Include(p => p.Assignment)
                                      .FirstOrDefault(p => p.ProcessId == processId);

            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if (processModel.Status == (int)ProcessStatusEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't get result with this status",
                    Success = false
                };
            }

            var records = _unit.AssignmentRecords
                                     .Find(r => r.LearningProcessId == processId)
                                     .ToList();

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            if (processModel.Assignment!.CanViewResult == false)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = _mapper.Map<List<AssignmentRecordResDto>>(records),
                    Success = true
                };
            }

            var assignmentResDtos = new List<AssignmentRecordResDto>();

            foreach (var record in records)
            {
                var resultModel = _mapper.Map<AssignmentRecordResDto>(record);
                var answerInfo = await _unit.AssignQues.GetAnswerInfoAsync(record.AssignQuesId, record.SubQueId);
                resultModel.AnswerInfo = answerInfo;

                assignmentResDtos.Add(resultModel);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = assignmentResDtos,
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, AssignRecordDto model)
        {
            var answerModel = _unit.AssignmentRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();
            try
            {
                if (answerModel.LearningProcessId != model.ProcessId)
                {
                    var changeResponse = await ChangeProcessAsync(id, model.ProcessId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (answerModel.AssignQuesId != model.AssignQuesId)
                {
                    var changeResponse = await ChangeAssignQuesAsync(id, model.AssignQuesId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (answerModel.SubQueId != model.SubId)
                {
                    var changeResponse = await ChangeSubAsync(id, model.SubId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if (answerModel.SelectedAnswer != model.SelectedAnswer)
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
