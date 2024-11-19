using System.Text.RegularExpressions;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Exams
{
    public class ToeicRecordService : IToeicRecordService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public ToeicRecordService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> ChangeProcessAsync(long id, long processId)
        {
            var recordModel = _unit.ToeicRecords.GetById(id);
            if (recordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic records",
                    Success = false
                };
            }

            var isExistProcess = _unit.LearningProcesses.IsExist(p => p.ProcessId == processId && p.ExamId.HasValue);
            if (!isExistProcess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicRecords.ChangeProcessAsync(recordModel, processId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change process",
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

        public async Task<Response> ChangeSelectedAnswerAsync(long id, string? selectedAnswer)
        {
            var recordModel = _unit.ToeicRecords
                                    .Include(s => s.SubToeic)
                                    .FirstOrDefault(s => s.RecordId == id);

            if (recordModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic records",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.ToeicRecords.ChangeSelectedAnswerAsync(recordModel, selectedAnswer);
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

        public async Task<Response> CreateAsync(ToeicRecordDto model)
        {
            var processModel = _unit.LearningProcesses
                                    .Find(p => p.ProcessId == model.ProcessId && p.ExamId.HasValue)
                                    .FirstOrDefault();

            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            var subQues = _unit.SubToeic.Include(s => s.Answer).FirstOrDefault(s => s.SubId == model.SubId);
            if (subQues == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any sub questions",
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

            var toeicModel = _mapper.Map<ToeicRecord>(model);
            toeicModel.IsCorrect = subQues.Answer == null ? false : subQues.Answer.CorrectAnswer == model.SelectedAnswer;

            _unit.ToeicRecords.Add(toeicModel);

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
            var toeicModel = _unit.ToeicRecords.GetById(id);
            if (toeicModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic records",
                    Success = false
                };
            }

            _unit.ToeicRecords.Remove(toeicModel);

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
            var toeicModels = _unit.ToeicRecords.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicRecordResDto>>(toeicModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var toeicModel = _unit.ToeicRecords.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<ToeicRecordResDto>(toeicModel),
                Success = true
            });
        }

        public async Task<Response> GetResultAsync(long processId)
        {
            var processModel = _unit.LearningProcesses.GetById(processId);
            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if (processModel.Status != (int)ProcessStatusEnum.Completed)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't get results when record isn't complete",
                    Success = false
                };
            }
            var records = await _unit.ToeicRecords.GetResultAsync(processId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<ToeicRecordResDto>>(records),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, ToeicRecordDto model)
        {
            var toeicModel = _unit.ToeicRecords.GetById(id);
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
                if (toeicModel.LearningProcessId != model.ProcessId)
                {
                    var response = await ChangeProcessAsync(id, model.ProcessId);
                    if (!response.Success) return response;
                }

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
    }
}
