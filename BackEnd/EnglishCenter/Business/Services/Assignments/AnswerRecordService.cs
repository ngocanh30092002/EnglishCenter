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
    public class AnswerRecordService : IAnswerRecordService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public AnswerRecordService(IUnitOfWork unit, IMapper mapper) 
        {
            _unit = unit;
            _mapper = mapper;

        }

        public async Task<Response> ChangeAssignQuesAsync(long id, long assignQueId)
        {
            var answerModel = _unit.AnswerRecords.GetById(id);
            if(answerModel == null)
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

            var isChangeSuccess = await _unit.AnswerRecords.ChangeAssignQuesAsync(answerModel, assignQueId);
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
            var answerModel = _unit.AnswerRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AnswerRecords.ChangeProcessAsync(answerModel, processId);
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
            var answerModel = _unit.AnswerRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AnswerRecords.ChangeSelectedAnswerAsync(answerModel, selectedAnswer);
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
            var answerModel = _unit.AnswerRecords.GetById(id);
            if (answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AnswerRecords.ChangeSubAsync(answerModel, subId);
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

        public async Task<Response> CreateAsync(AnswerRecordDto model)
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

            if(assignQueModel.AssignmentId != processModel.AssignmentId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "AssignQues must belong to Assignment",
                    Success = false
                };
            }

            var pattern = "^[ABCDabcd]+$";
            if(!Regex.IsMatch(model.SelectedAnswer, pattern))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Selected Answer isn't valid",
                    Success = false
                };
            }


            var isExistSub = await _unit.AnswerRecords.IsExistSubAsync((QuesTypeEnum)assignQueModel.Type, model.SubId);
            if (!isExistSub)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Sub Id isn't valid",
                    Success = false
                };
            }

            var isCorrectAnswer = await _unit.AssignQues.IsCorrectAnswerAsync(assignQueModel, model.SelectedAnswer, model.SubId);

            var existAnswerRecord = _unit.AnswerRecords
                                        .Find(a => a.LearningProcessId == model.ProcessId && 
                                                    a.AssignQuesId == model.AssignQuesId && 
                                                    a.SubQueId == model.SubId)
                                        .FirstOrDefault();
            
            if(existAnswerRecord != null)
            {
                _unit.AnswerRecords.Remove(existAnswerRecord);
            }

            var answerEntity = _mapper.Map<AnswerRecord>(model);

            answerEntity.IsCorrect = isCorrectAnswer;
            _unit.AnswerRecords.Add(answerEntity);
            
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
            var answerModel = _unit.AnswerRecords.GetById(id);

            if(answerModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any answer records",
                    Success = false
                };
            }

            _unit.AnswerRecords.Remove(answerModel);
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
            var answerModels = _unit.AnswerRecords.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AnswerRecordResDto>>(answerModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var answerModel = _unit.AnswerRecords.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AnswerRecordResDto>(answerModel),
                Success = true
            });
        }

        public Task<Response> GetByProcessIdAsync(long processId)
        {
            var answerModels = _unit.AnswerRecords.Find(a => a.LearningProcessId == processId).ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AnswerRecordResDto>>(answerModels),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, AnswerRecordDto model)
        {
            var answerModel = _unit.AnswerRecords.GetById(id);
            if(answerModel == null)
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
                if(answerModel.LearningProcessId != model.ProcessId)
                {
                    var changeResponse = await ChangeProcessAsync(id, model.ProcessId);
                    if (!changeResponse.Success) return changeResponse;
                }

                if(answerModel.AssignQuesId != model.AssignQuesId)
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
            catch(Exception ex)
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
