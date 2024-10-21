using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace EnglishCenter.Business.Services.HomeworkTasks
{
    public class HwSubmissionService : IHwSubmissionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IHwSubRecordService _subRecordService;

        public HwSubmissionService(IUnitOfWork unit, IMapper mapper, IHwSubRecordService subRecordService)
        {
            _unit = unit;
            _mapper = mapper;
            _subRecordService = subRecordService;
        }

        public async Task<Response> ChangeDateAsync(long hwSubId, string dateTime)
        {
            var submitModel = _unit.HwSubmissions.GetById(hwSubId);
            if(submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            if(!DateTime.TryParse(dateTime, out DateTime date))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid date time",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubmissions.ChangeDateAsync(submitModel, date);
            if(!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change date",
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

        public async Task<Response> ChangeEnrollAsync(long hwSubId, long enrollId)
        {
            var submitModel = await _unit.HwSubmissions
                                .Include(s => s.Homework)
                                .FirstOrDefaultAsync(s => s.SubmissionId == hwSubId);

            if(submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var enrollment = _unit.Enrollment.GetById(enrollId);
            if(enrollment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            if(enrollment.ClassId != submitModel.Homework.ClassId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change enrollment",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubmissions.ChangeEnrollAsync(submitModel, enrollId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change enrollment",
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

        public async Task<Response> ChangeFeedbackAsync(long hwSubId, string feedback)
        {
            var submitModel = _unit.HwSubmissions.GetById(hwSubId);
            if (submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubmissions.ChangeFeedbackAsync(submitModel, feedback);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change feedback",
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

        public async Task<Response> ChangeHomeworkAsync(long hwSubId, long homeworkId)
        {
            var submitModel = await _unit.HwSubmissions
                                        .Include(s => s.Enrollment)
                                        .FirstOrDefaultAsync(s => s.SubmissionId == hwSubId);

            if (submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var homeworkModel = _unit.Homework.GetById(homeworkId);
            if(homeworkModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if(homeworkModel.ClassId != submitModel.Enrollment.ClassId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change homework unless they are in the same class",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubmissions.ChangeHomeworkAsync(submitModel, homeworkId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change homework",
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

        public async Task<Response> ChangeIsPassAsync(long hwSubId, bool isPass)
        {
            var submitModel = _unit.HwSubmissions.GetById(hwSubId);
            if(submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubmissions.ChangeIsPassAsync(submitModel, isPass);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change pass status",
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

        public async Task<Response> CreateAsync(HwSubmissionDto model)
        {
            var homeworkModel = _unit.Homework.GetById(model.HomeworkId);
            if(homeworkModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            var enrollment = _unit.Enrollment.GetById(model.EnrollId);
            if(enrollment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollment",
                    Success = false
                };
            }

            if(enrollment.StatusId != (int)EnrollEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The user's status is currently unable to submit homework",
                    Success = false
                };
            }

            if(enrollment.ClassId != homeworkModel.ClassId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Cannot submit homework from other classes",
                    Success = false
                };
            }
            
            var currentTime = DateTime.Now;
            if(!string.IsNullOrEmpty(model.Date) && DateTime.TryParse(model.Date, out DateTime time))
            {
                currentTime = time;
            }

            if(homeworkModel.StartTime > currentTime)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The homework is not open yet, please submit it later.",
                    Success = false
                };
            }

            var submitEntity = _mapper.Map<HwSubmission>(model);

            submitEntity.Date = currentTime;
            submitEntity.IsPass = false;

            if (homeworkModel.StartTime <= currentTime && currentTime <= homeworkModel.EndTime)
            {
                submitEntity.SubmitStatus = (int)SubmitStatusEnum.OnTime;
            }
            else if (homeworkModel.EndTime < currentTime && currentTime <= homeworkModel.EndTime.AddDays(homeworkModel.LateSubmitDays))
            {
                submitEntity.SubmitStatus = (int)SubmitStatusEnum.Late;
            }
            else
            {
                submitEntity.SubmitStatus = (int)SubmitStatusEnum.Overdue;
            }

            _unit.HwSubmissions.Add(submitEntity);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long hwSubId)
        {
            var submitModel = await _unit.HwSubmissions
                                        .Include(s => s.SubRecords)
                                        .FirstOrDefaultAsync(s => s.SubmissionId == hwSubId);

            if(submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();
            try
            {
                var subIds = submitModel.SubRecords.Select(s => s.RecordId).ToList();
                foreach (var subId in subIds)
                {
                    var response = await _subRecordService.DeleteAsync(subId);
                    if (!response.Success) return response;
                }

                _unit.HwSubmissions.Remove(submitModel);

                await _unit.CompleteAsync();
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

        public Task<Response> GetAllAsync()
        {
            // Todo: change resdto
            var submitModels = _unit.HwSubmissions.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HwSubmissionDto>>(submitModels),
                Success = false
            });
        }

        public Task<Response> GetAsync(long hwSubId)
        {
            // Todo: change resdto

            var submitModel = _unit.HwSubmissions.GetById(hwSubId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HwSubmissionDto>(submitModel),
                Success = false
            }); 
        }

        public async Task<Response> HandleSubmitHomework(long hwSubId, HwSubmissionDto model)
        {
            var submissionModel = await _unit.HwSubmissions
                                       .Include(s => s.Homework)
                                       .FirstOrDefaultAsync(s => s.SubmissionId == hwSubId);

            if(submissionModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();
            try
            {
                if(model.Answers != null && model.Answers.Count > 0) 
                {
                    foreach(var answer in model.Answers)
                    {
                        var response = await _subRecordService.CreateAsync(answer);
                        if (!response.Success) return response;
                    }
                }

                var correctNum = _unit.HwSubRecords.Find(r => r.IsCorrect == true && r.SubmissionId == hwSubId).Count();
                var numQues = await _unit.HomeQues.GetNumberByHomeworkAsync(submissionModel.HomeworkId);
                bool isQualified = ((correctNum * 100.0) / numQues) > submissionModel.Homework.AchievedPercentage;

                submissionModel.IsPass = isQualified;
                submissionModel.Date = DateTime.Now;

                await _unit.CompleteAsync();
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

        public async Task<Response> UpdateAsync(long hwSubId, HwSubmissionDto model)
        {
            var submitModel = _unit.HwSubmissions.GetById(hwSubId);
            if(submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if(submitModel.EnrollId != model.EnrollId)
                {
                    var response = await ChangeEnrollAsync(hwSubId, model.EnrollId);
                    if (!response.Success) return response;
                }

                if(submitModel.HomeworkId != model.HomeworkId)
                {
                    var response = await ChangeHomeworkAsync(hwSubId, model.HomeworkId);
                    if (!response.Success) return response;
                }

                if (!string.IsNullOrEmpty(model.Date) && submitModel.Date.ToString() != model.Date)
                {
                    var response = await ChangeDateAsync(hwSubId, model.Date);
                    if (!response.Success) return response;
                }

                if(!string.IsNullOrEmpty(model.Feedback) && submitModel.FeedBack != model.Feedback)
                {
                    var response = await ChangeFeedbackAsync(hwSubId, model.Feedback);
                    if (!response.Success) return response;
                }

                if(model.IsPass.HasValue && submitModel.IsPass != model.IsPass.Value)
                {
                    var response = await ChangeIsPassAsync(hwSubId, model.IsPass.Value);
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
