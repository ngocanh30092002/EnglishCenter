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
    public class HwSubmissionService : IHwSubmissionService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IHwSubRecordService _subRecordService;
        private readonly IUserService _userService;

        public HwSubmissionService(IUnitOfWork unit, IMapper mapper, IHwSubRecordService subRecordService, IUserService userService)
        {
            _unit = unit;
            _mapper = mapper;
            _subRecordService = subRecordService;
            _userService = userService;
        }

        public async Task<Response> ChangeDateAsync(long hwSubId, string dateTime)
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

            if (!DateTime.TryParse(dateTime, out DateTime date))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid date time",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.HwSubmissions.ChangeDateAsync(submitModel, date);
            if (!isChangeSuccess)
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

            if (submitModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var lessonModel = _unit.Lessons.GetById(submitModel.Homework.LessonId);
            var enrollment = _unit.Enrollment.GetById(enrollId);

            if (enrollment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            if (enrollment.ClassId != lessonModel.ClassId)
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
                                        .Include(s => s.Homework)
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
            if (homeworkModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }


            var lessonModel = _unit.Lessons.GetById(submitModel.Homework.LessonId);

            if (lessonModel.ClassId != submitModel.Enrollment.ClassId)
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
            if (submitModel == null)
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
            var homeworkModel = _unit.Homework
                                    .Include(s => s.Lesson)
                                    .FirstOrDefault(s => s.HomeworkId == model.HomeworkId);

            if (homeworkModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            if (!model.EnrollId.HasValue)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollment",
                    Success = false
                };
            }

            var enrollment = _unit.Enrollment.GetById(model.EnrollId.Value);
            if (enrollment == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollment",
                    Success = false
                };
            }

            if (enrollment.StatusId != (int)EnrollEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "The user's status is currently unable to submit homework",
                    Success = false
                };
            }

            if (enrollment.ClassId != homeworkModel.Lesson.ClassId)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Cannot submit homework from other classes",
                    Success = false
                };
            }

            var currentTime = DateTime.Now;
            if (!string.IsNullOrEmpty(model.Date) && DateTime.TryParse(model.Date, out DateTime time))
            {
                currentTime = time;
            }

            if (homeworkModel.StartTime > currentTime)
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
            submitEntity.SubmitStatus = (int)SubmitStatusEnum.Ongoing;

            _unit.HwSubmissions.Add(submitEntity);

            await _unit.CompleteAsync();
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = submitEntity.SubmissionId,
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long hwSubId)
        {
            var submitModel = await _unit.HwSubmissions
                                        .Include(s => s.SubRecords)
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
            var submitModels = _unit.HwSubmissions.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<HwSubmissionDto>>(submitModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long hwSubId)
        {
            var submitModel = _unit.HwSubmissions.GetById(hwSubId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HwSubmissionDto>(submitModel),
                Success = true
            });
        }

        public async Task<Response> GetByEnrollAsync(long enrollId, long homeworkId)
        {
            var submissionModels = await _unit.HwSubmissions
                                        .Include(s => s.Homework)
                                        .Include(s => s.SubRecords)
                                        .Where(s => s.EnrollId == enrollId && s.HomeworkId == homeworkId)
                                        .ToListAsync();

            var resDtos = new List<HwSubmissionResDto>();

            foreach (var item in submissionModels)
            {
                var totalNum = item.SubRecords.Count;
                var correctNum = item.SubRecords.Where(s => s.IsCorrect == true).Count();
                var percentage = item.Homework.AchievedPercentage;
                var currentPercentage = Math.Ceiling(correctNum * 100.0 * 100.0 / totalNum) / 100;

                var scoreResDto = new HomeworkScoreResDto()
                {
                    Correct = correctNum,
                    InCorrect = totalNum - correctNum,
                    Total = totalNum,
                    Achieve_Percentage = percentage,
                    Current_Percentage = currentPercentage,
                    IsPass = item.IsPass
                };

                var resDto = _mapper.Map<HwSubmissionResDto>(item);
                resDto.Score = scoreResDto;

                resDtos.Add(resDto);
            }


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            };
        }

        public Task<Response> GetNumberAttemptAsync(long enrollId, long homeworkId)
        {
            var numberAttempts = _unit.HwSubmissions
                                      .Find(s => s.EnrollId == enrollId &&
                                                 s.HomeworkId == homeworkId &&
                                                 s.SubmitStatus != (int)SubmitStatusEnum.Ongoing)
                                      .Count();


            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = numberAttempts,
                Success = true
            });
        }

        public async Task<Response> GetOngoingAsync(long enrollId, long homeworkId)
        {
            var enrollModel = _unit.Enrollment.GetById(enrollId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            var submissionModel = await _unit.HwSubmissions
                                             .Include(s => s.Homework)
                                             .Where(s => s.EnrollId == enrollId &&
                                                         s.SubmitStatus == (int)SubmitStatusEnum.Ongoing &&
                                                         s.HomeworkId == homeworkId)
                                             .FirstOrDefaultAsync();

            if (submissionModel != null)
            {
                DateTime endTime = submissionModel.Date.Add(submissionModel.Homework.Time.ToTimeSpan());

                if (endTime <= DateTime.Now)
                {
                    var response = await HandleSubmitHomework(submissionModel.SubmissionId, new HwSubmissionDto()
                    {
                        HomeworkId = homeworkId,
                    });

                    if (!response.Success) return response;

                    return new Response
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Message = null,
                        Success = true
                    };
                }
            }

            return new Response
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<HwSubmissionResDto>(submissionModel),
                Success = true
            };

        }

        public Task<Response> IsSubmittedAsync(long hwSubId)
        {
            var hwSubmissionModel = _unit.HwSubmissions.GetById(hwSubId);

            if (hwSubmissionModel == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any submissions",
                    Success = false
                });
            }

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = hwSubmissionModel.SubmitStatus != (int)SubmitStatusEnum.Ongoing,
                Success = true
            });
        }

        public async Task<Response> HandleSubmitHomework(long hwSubId, HwSubmissionDto model)
        {
            var submissionModel = await _unit.HwSubmissions
                                       .Include(s => s.Homework)
                                       .FirstOrDefaultAsync(s => s.SubmissionId == hwSubId);

            if (submissionModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework submissions",
                    Success = false
                };
            }

            var homeworkModel = submissionModel.Homework;

            await _unit.BeginTransAsync();
            try
            {
                if (model.Answers != null && model.Answers.Count > 0)
                {
                    foreach (var answer in model.Answers)
                    {
                        var response = await _subRecordService.CreateAsync(answer);
                        if (!response.Success) return response;
                    }
                }

                var correctNum = _unit.HwSubRecords.Find(r => r.IsCorrect == true && r.SubmissionId == hwSubId).Count();
                var numQues = await _unit.HomeQues.GetNumberByHomeworkAsync(submissionModel.HomeworkId);
                bool isQualified = ((correctNum * 100.0) / numQues) > submissionModel.Homework.AchievedPercentage;
                var currentTime = DateTime.Now;

                if (homeworkModel.StartTime <= currentTime && currentTime <= homeworkModel.EndTime)
                {
                    submissionModel.SubmitStatus = (int)SubmitStatusEnum.OnTime;
                }
                else if (homeworkModel.EndTime < currentTime && currentTime <= homeworkModel.EndTime.AddDays(homeworkModel.LateSubmitDays))
                {
                    submissionModel.SubmitStatus = (int)SubmitStatusEnum.Late;
                }
                else
                {
                    submissionModel.SubmitStatus = (int)SubmitStatusEnum.Overdue;
                }

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

        public async Task<bool> IsInChargeAsync(string userId, long subId)
        {
            var submissionModel = await _unit.HwSubmissions.Include(s => s.Homework)
                                                            .FirstOrDefaultAsync(s => s.SubmissionId == subId);
            if (submissionModel == null) return false;

            return await _unit.Homework.IsInChargeAsync(submissionModel.Homework, userId);
        }

        public async Task<Response> UpdateAsync(long hwSubId, HwSubmissionDto model)
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

            await _unit.BeginTransAsync();

            try
            {
                if (model.EnrollId.HasValue && submitModel.EnrollId != model.EnrollId)
                {
                    var response = await ChangeEnrollAsync(hwSubId, model.EnrollId.Value);
                    if (!response.Success) return response;
                }

                if (model.HomeworkId.HasValue && submitModel.HomeworkId != model.HomeworkId)
                {
                    var response = await ChangeHomeworkAsync(hwSubId, model.HomeworkId.Value);
                    if (!response.Success) return response;
                }

                if (!string.IsNullOrEmpty(model.Date) && submitModel.Date.ToString() != model.Date)
                {
                    var response = await ChangeDateAsync(hwSubId, model.Date);
                    if (!response.Success) return response;
                }

                if (!string.IsNullOrEmpty(model.Feedback) && submitModel.FeedBack != model.Feedback)
                {
                    var response = await ChangeFeedbackAsync(hwSubId, model.Feedback);
                    if (!response.Success) return response;
                }

                if (model.IsPass.HasValue && submitModel.IsPass != model.IsPass.Value)
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

        public async Task<Response> GetScoreAsync(long hwSubId)
        {
            var submissionModel = await _unit.HwSubmissions
                                             .Include(s => s.Homework)
                                             .FirstOrDefaultAsync(s => s.SubmissionId == hwSubId);

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
                    Message = "This process is in progress so the score cannot be retrieved.",
                    Success = false
                };
            }

            if (submissionModel.Homework.Type == 1)
            {
                var correctNumber = _unit.HwSubRecords.Find(r => r.SubmissionId == submissionModel.SubmissionId && r.IsCorrect).Count();
                var totalNumber = await _unit.HomeQues.GetNumberByHomeworkAsync(submissionModel.HomeworkId);
                var achievedPercentage = submissionModel.Homework.AchievedPercentage;
                var currentPercentage = Math.Ceiling(correctNumber * 100.0 * 100.0 / totalNumber) / 100;

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = new AssignmentScoreResDto()
                    {
                        Correct = correctNumber,
                        InCorrect = totalNumber - correctNumber,
                        Total = totalNumber,
                        Achieve_Percentage = achievedPercentage,
                        Current_Percentage = currentPercentage,
                        IsPass = currentPercentage >= achievedPercentage
                    }
                };
            }
            else
            {
                var res = new ToeicScoreResDto()
                {
                    Part1 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part1),
                    Part2 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part2),
                    Part3 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part3),
                    Part4 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part4),
                    Part5 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part5),
                    Part6 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part6),
                    Part7 = await _unit.HwSubRecords.GetNumCorrectWithPartAsync(hwSubId, (int)PartEnum.Part7),
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
                               .Find(s => s.SubmissionId == submissionModel.SubmissionId)
                               .ToList();

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            var hwRecordResDtos = new List<HwSubRecordResDto>();

            //foreach (var record in records)
            //{
            //    var resultModel = _mapper.Map<HwSubRecordResDto>(record);
            //    var answerInfo = await _unit.AssignQues.GetAnswerInfoAsync(record.AssignQuesId, record.SubQueId);
            //    resultModel.AnswerInfo = answerInfo;

            //    assignmentResDtos.Add(resultModel);
            //}

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = hwRecordResDtos,
                Success = true
            };
        }

        public async Task<Response> GetByEnrollHistoryAsync(long enrollId)
        {
            var submissionModels = await _unit.HwSubmissions
                                        .Include(s => s.Homework)
                                        .Include(s => s.SubRecords)
                                        .Where(s => s.EnrollId == enrollId && s.SubmitStatus != (int)SubmitStatusEnum.Ongoing)
                                        .ToListAsync();

            var resDtos = new List<HwSubmissionResDto>();

            foreach (var item in submissionModels)
            {
                var totalNum = item.SubRecords.Count;
                var correctNum = item.SubRecords.Where(s => s.IsCorrect == true).Count();
                var percentage = item.Homework.AchievedPercentage;
                var currentPercentage = Math.Ceiling(correctNum * 100.0 * 100.0 / totalNum) / 100;

                var scoreResDto = new HomeworkScoreResDto()
                {
                    Correct = correctNum,
                    InCorrect = totalNum - correctNum,
                    Total = totalNum,
                    Achieve_Percentage = percentage,
                    Current_Percentage = currentPercentage,
                    IsPass = item.IsPass
                };

                var resDto = _mapper.Map<HwSubmissionResDto>(item);
                resDto.Score = scoreResDto;

                resDtos.Add(resDto);
            }


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = resDtos,
                Success = true
            };
        }

        public async Task<Response> GetByHomeworkAsync(long homeworkId)
        {
            var hwSubmissions = _unit.HwSubmissions
                                     .Include(s => s.Enrollment)
                                     .Include(s => s.Homework)
                                     .Where(s => s.HomeworkId == homeworkId)
                                     .ToList();

            var hwSubResDtos = new List<HwSubmissionResDto>();

            foreach (var submission in hwSubmissions)
            {
                var subResDto = _mapper.Map<HwSubmissionResDto>(submission);
                var hwScoreModel = await _unit.HwSubRecords.GetScoreBySubmissionAsync(subResDto.SubmissionId);
                var fullInfoRes = await _userService.GetUserFullInfoAsync(submission.Enrollment.UserId);
                var fullInfo = fullInfoRes.Message as UserInfoResDto;
                subResDto.Score = hwScoreModel;
                subResDto.UserName = fullInfo!.FirstName + " " + fullInfo!.LastName;
                subResDto.Email = fullInfo!.Email;
                subResDto.ImageUrl = fullInfo.Image == null ? null : fullInfo.Image.Replace("\\", "/");

                hwSubResDtos.Add(subResDto);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = hwSubResDtos,
                Success = true
            };
        }
    }
}
