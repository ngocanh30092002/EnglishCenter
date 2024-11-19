using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Assignments
{
    public class LearningProcessService : ILearningProcessService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IAssignmentRecordService _assignmentRecordService;
        private readonly IToeicRecordService _toeicRecordService;

        public LearningProcessService(IUnitOfWork unit, IMapper mapper, IAssignmentRecordService assignmentRecordService, IToeicRecordService toeicRecordService)
        {
            _unit = unit;
            _mapper = mapper;
            _assignmentRecordService = assignmentRecordService;
            _toeicRecordService = toeicRecordService;
        }

        public async Task<Response> ChangeEndTimeAsync(long id, string dateTime)
        {
            var processModel = _unit.LearningProcesses.GetById(id);
            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if (!DateTime.TryParse(dateTime, out DateTime time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time isn't valid",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.LearningProcesses.ChangeEndTimeAsync(processModel, time);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't set end time",
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

        public async Task<Response> ChangeStartTimeAsync(long id, string dateTime)
        {
            var processModel = _unit.LearningProcesses.GetById(id);
            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if (!DateTime.TryParse(dateTime, out DateTime time))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time isn't valid",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.LearningProcesses.ChangeStartTimeAsync(processModel, time);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't set start time",
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

        public async Task<Response> ChangeStatusAsync(long id, int status)
        {
            if (!Enum.IsDefined(typeof(ProcessStatusEnum), status))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Status is not valid",
                    Success = false
                };
            }

            var processModel = _unit.LearningProcesses.GetById(id);
            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.LearningProcesses.ChangeStatusAsync(processModel, (ProcessStatusEnum)status);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change status",
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

        public async Task<Response> CreateAsync(LearningProcessDto model)
        {
            var enrollModel = await _unit.Enrollment
                                .Include(e => e.Class)
                                .FirstOrDefaultAsync(e => e.EnrollId == model.EnrollId);

            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            if (enrollModel.StatusId != (int)EnrollEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Class has not started yet",
                    Success = false
                };
            }

            if (!(model.AssignmentId == null ^ model.ExamId == null))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid input",
                    Success = false
                };
            }

            var courseModel = _unit.Courses.GetById(enrollModel.Class.CourseId);
            var entity = new LearningProcess();

            if (courseModel.IsSequential)
            {
                if (model.AssignmentId.HasValue)
                {
                    var response = await CreateWithAssignmentAsync(model);
                    if (!response.Success) return response;
                }
                else
                {
                    var response = await CreateWithExamAsync(model);
                    if (!response.Success) return response;
                }
            }
            else
            {
                if (model.AssignmentId.HasValue)
                {
                    var isExistAssignment = _unit.Assignments.IsExist(a => a.AssignmentId == model.AssignmentId);
                    if (!isExistAssignment)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't find any assignments",
                            Success = false
                        };
                    }

                    var isExistProcess = _unit.LearningProcesses.IsExist(c => c.EnrollId == model.EnrollId &&
                                                                              c.AssignmentId == model.AssignmentId &&
                                                                              c.Status == (int)ProcessStatusEnum.Ongoing);

                    if (isExistProcess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't create new process",
                            Success = false
                        };
                    }
                }
                else
                {
                    var isExistExam = _unit.Examinations.IsExist(e => e.ExamId == model.ExamId);
                    if (!isExistExam)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't find any examination",
                            Success = false
                        };
                    }
                    var isExistProcess = _unit.LearningProcesses.IsExist(c => c.EnrollId == model.EnrollId &&
                                                                             c.ExamId == model.ExamId);

                    if (isExistProcess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't create new process",
                            Success = false
                        };
                    }
                }
            }

            entity = _mapper.Map<LearningProcess>(model);
            _unit.LearningProcesses.Add(entity);

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = entity.ProcessId,
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var processModel = _unit.LearningProcesses
                                    .Include(a => a.Assignment)
                                    .Include(a => a.Examination)
                                    .FirstOrDefault(p => p.ProcessId == id);
            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if (processModel.Status == (int)ProcessStatusEnum.NotAchieved || processModel.Status == (int)ProcessStatusEnum.Ongoing)
            {
                _unit.LearningProcesses.Remove(processModel);
                await _unit.CompleteAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            else
            {
                var courseContentId = processModel.AssignmentId == null ? processModel.Examination!.ContentId : processModel.Assignment!.CourseContentId;
                var courseContentModel = _unit.CourseContents.GetById(courseContentId);

                var nextAssignments = _unit.Assignments
                                        .Include(a => a.CourseContent)
                                        .Where(a => a.CourseContent.CourseId == courseContentModel.CourseId
                                                    && (
                                                            (a.CourseContent.NoNum == courseContentModel.NoNum && a.NoNum > processModel.Assignment.NoNum) ||
                                                            (a.CourseContent.NoNum > courseContentModel.NoNum)
                                                        )
                                                )
                                        .Select(a => a.AssignmentId)
                                        .Distinct();

                var isExist = _unit.LearningProcesses.IsExist(a => a.EnrollId == processModel.EnrollId && nextAssignments.Any(id => id == a.AssignmentId));
                if (isExist)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't delete this processes",
                        Success = false
                    };
                }

                var otherCourseContentIds = _unit.CourseContents
                                              .Find(p => p.CourseId == courseContentModel.CourseId &&
                                                         p.NoNum > courseContentModel.NoNum
                                                         && p.Type != (int)CourseContentTypeEnum.Normal)
                                              .Select(p => p.ContentId);

                isExist = await _unit.LearningProcesses
                                     .Include(p => p.Examination)
                                     .AnyAsync(p => p.Examination != null && otherCourseContentIds.Any(c => c == p.Examination.ContentId));

                if (isExist)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't delete this processes",
                        Success = false
                    };
                }

                _unit.LearningProcesses.Remove(processModel);
                await _unit.CompleteAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }

        }

        public Task<Response> GetAllAsync()
        {
            var processes = _unit.LearningProcesses.GetAll();

            // Todo: change to resdto
            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<LearningProcessResDto>>(processes),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var process = _unit.LearningProcesses.GetById(id);

            // Todo: change to resdto
            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<LearningProcessResDto>(process),
                Success = true
            });
        }

        public Task<Response> IsSubmittedAsync(long id)
        {
            var processModel = _unit.LearningProcesses.GetById(id);

            if (processModel == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                });
            }

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = processModel.Status != (int)ProcessStatusEnum.Ongoing,
                Success = true
            });
        }

        public async Task<Response> GetHisProcessesAsync(long enrollId, long? assignmentId, long? examId)
        {
            if (!(assignmentId == null ^ examId == null))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid parameter",
                    Success = false
                };
            };

            var enrollModel = _unit.Enrollment.GetById(enrollId);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "",
                    Success = false
                };
            }

            var processResDtos = new List<LearningProcessResDto>();

            if (assignmentId.HasValue)
            {
                var hisProcesses = _unit.LearningProcesses
                                        .Find(p => p.EnrollId == enrollModel.EnrollId &&
                                                   p.AssignmentId == assignmentId.Value &&
                                                   p.Status != (int)ProcessStatusEnum.Ongoing)
                                        .OrderByDescending(p => p.StartTime)
                                        .ToList();

                foreach (var hisProcess in hisProcesses)
                {
                    var correctNum = _unit.AssignmentRecords.Find(a => a.LearningProcessId == hisProcess.ProcessId && a.IsCorrect).Count();
                    var numQues = await _unit.AssignQues.GetNumberByAssignmentAsync(assignmentId!.Value);

                    var processResModel = _mapper.Map<LearningProcessResDto>(hisProcess);
                    processResModel.Result = $"{correctNum}/{numQues}";

                    processResDtos.Add(processResModel);
                }
            }
            else
            {
                var hisProcesses = _unit.LearningProcesses
                                        .Find(p => p.EnrollId == enrollModel.EnrollId &&
                                                   p.ExamId == examId!.Value &&
                                                   p.Status != (int)ProcessStatusEnum.Ongoing)
                                        .OrderByDescending(p => p.StartTime)
                                        .ToList();

                foreach (var hisProcess in hisProcesses)
                {
                    var correctNum = _unit.ToeicRecords.Find(t => t.LearningProcessId == hisProcess.ProcessId && t.IsCorrect).Count();

                    var processResModel = _mapper.Map<LearningProcessResDto>(hisProcess);
                    processResModel.Result = $"{correctNum}/200";

                    processResDtos.Add(processResModel);
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = processResDtos,
                Success = true
            };
        }

        public Task<Response> GetNumberAttemptedAsync(long enrollId, long assignmentId)
        {
            var processModel = _unit.LearningProcesses
                                    .Find(p => p.EnrollId == enrollId &&
                                               p.AssignmentId == assignmentId &&
                                               p.Status != (int)ProcessStatusEnum.Ongoing);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = processModel.Count(),
                Success = true
            });
        }

        public async Task<Response> GetScoreByProcessAsync(long id)
        {
            var processModel = await _unit.LearningProcesses
                                          .Include(p => p.Assignment)
                                          .Include(p => p.Examination)
                                          .FirstOrDefaultAsync(p => p.ProcessId == id);

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
                    Message = "This process is in progress so the score cannot be retrieved.",
                    Success = false
                };
            }

            if (processModel.AssignmentId.HasValue)
            {
                var correctNum = _unit.AssignmentRecords.Find(a => a.LearningProcessId == processModel.ProcessId && a.IsCorrect).Count();
                var totalNum = await _unit.AssignQues.GetNumberByAssignmentAsync(processModel.AssignmentId.Value);
                var achievedPercentage = processModel.Assignment!.AchievedPercentage;
                var currentPercentage = Math.Ceiling(correctNum * 100.0 * 100.0 / totalNum) / 100;

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = new AssignmentScoreResDto()
                    {
                        Correct = correctNum,
                        InCorrect = totalNum - correctNum,
                        Total = totalNum,
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
                    Part1 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part1),
                    Part2 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part2),
                    Part3 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part3),
                    Part4 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part4),
                    Part5 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part5),
                    Part6 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part6),
                    Part7 = await _unit.ToeicRecords.GetNumCorrectRecordsWithPartAsync(id, (int)PartEnum.Part7),
                };

                var listeningCon = await _unit.ToeicConversion.GetByNumberCorrectAsync(res.Part1 + res.Part2 + res.Part3 + res.Part4, ToeicEnum.Listening);
                var readingCon = await _unit.ToeicConversion.GetByNumberCorrectAsync(res.Part5 + res.Part6 + res.Part7, ToeicEnum.Reading);

                res.Listening = listeningCon == null ? 0 : listeningCon.EstimatedScore;
                res.Reading = readingCon == null ? 0 : readingCon.EstimatedScore;

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = res
                };
            }
        }

        public async Task<Response> GetOngoingAsync(long enrollId, long? assignmentId, long? examId)
        {
            if (!(assignmentId == null ^ examId == null))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid parameter",
                    Success = false
                };
            };

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

            if (assignmentId.HasValue)
            {
                var processModel = _unit.LearningProcesses
                                        .Include(p => p.Assignment)
                                        .Where(p => p.EnrollId == enrollModel.EnrollId &&
                                                             p.Status == (int)ProcessStatusEnum.Ongoing &&
                                                             p.AssignmentId == assignmentId.Value)
                                        .FirstOrDefault();
                if (processModel != null)
                {
                    DateTime endTime = processModel.StartTime.Add(processModel.Assignment.Time.ToTimeSpan());

                    if (endTime <= DateTime.Now)
                    {
                        var response = await HandleSubmitProcessAsync(processModel.ProcessId, new LearningProcessDto()
                        {
                            AssignmentId = processModel.AssignmentId
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
                    Message = _mapper.Map<LearningProcessResDto>(processModel),
                    Success = true
                };
            }
            else
            {
                var processModel = _unit.LearningProcesses
                                        .Include(p => p.Examination)
                                        .Where(p => p.EnrollId == enrollModel.EnrollId &&
                                                             p.Status == (int)ProcessStatusEnum.Ongoing &&
                                                             p.ExamId == examId!.Value)
                                        .FirstOrDefault();

                if (processModel != null)
                {
                    DateTime endTime = processModel.StartTime.Add(processModel.Examination.Time.ToTimeSpan());

                    if (endTime <= DateTime.Now)
                    {
                        var response = await HandleSubmitProcessAsync(processModel.ProcessId, new LearningProcessDto()
                        {
                            ExamId = processModel.ExamId
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
                    Message = _mapper.Map<LearningProcessResDto>(processModel),
                    Success = true
                };
            }
        }

        public async Task<Response> HandleSubmitProcessAsync(long id, LearningProcessDto model)
        {
            var processModel = _unit.LearningProcesses
                                    .Include(p => p.Assignment)
                                    .Include(p => p.Enrollment)
                                    .FirstOrDefault(p => p.ProcessId == id);

            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if (processModel.Status != (int)ProcessStatusEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Process isn't valid",
                    Success = false
                };
            }

            if (!(model.AssignmentId == null ^ model.ExamId == null))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid input",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (model.AssignmentId.HasValue)
                {
                    var response = await HandleSubmitWithAssignmentAsync(processModel, model);
                    if (!response.Success) return response;
                }
                else
                {
                    var response = await HandleSubmitWithExamAsync(processModel, model);
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

        public Task<Response> GetStatusExamAsync(long enrollId, long examId)
        {
            var enrollModel = _unit.Enrollment.GetById(enrollId);
            if (enrollModel == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollment",
                    Success = false
                });
            }

            var processModel = _unit.LearningProcesses.Find(p => p.EnrollId == enrollId && p.ExamId == examId).FirstOrDefault();
            if (processModel == null)
            {
                return Task.FromResult(new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = null,
                    Success = true
                });
            }

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = new
                {
                    ProcessId = processModel.ProcessId,
                    Status = ((ProcessStatusEnum)processModel.Status).ToString(),
                },
                Success = true
            });
        }
        public async Task<Response> GetStatusLessonAsync(long id, long? assignmentId, long? examId)
        {
            var enrollModel = _unit.Enrollment.Include(e => e.Class).FirstOrDefault(e => e.EnrollId == id);
            if (enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollment",
                    Success = false
                };
            }

            var status = await IsStatusLessonAsync(enrollModel, assignmentId, examId);


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = status.ToString(),
                Success = true
            };
        }

        public async Task<LessonStatusEnum> IsStatusLessonAsync(Enrollment enroll, long? assignmentId, long? examId)
        {
            if (enroll.StatusId != (int)EnrollEnum.Ongoing) return LessonStatusEnum.Locked;
            if (!(assignmentId == null ^ examId == null)) return LessonStatusEnum.Locked;

            var courseModel = _unit.Courses.GetById(enroll.Class.CourseId);
            if (courseModel == null) return LessonStatusEnum.Locked;

            if (assignmentId.HasValue)
            {
                var assignment = await _unit.Assignments
                                            .Include(a => a.CourseContent)
                                            .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId.Value);

                if (assignment == null) return LessonStatusEnum.Locked;

                return await IsStatusWithAssignmentAsync(enroll, assignment, courseModel.IsSequential);
            }
            else
            {
                var examModel = await _unit.Examinations
                                           .Include(a => a.CourseContent)
                                           .FirstOrDefaultAsync(a => a.ExamId == examId);

                if (examModel == null) return LessonStatusEnum.Locked;

                return await IsStatusWithExamAsync(enroll, examModel, courseModel.IsSequential);
            }
        }

        public async Task<Response> UpdateAsync(long id, LearningProcessDto model)
        {
            var processModel = _unit.LearningProcesses.GetById(id);

            if (processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (processModel.Status != model.Status)
                {
                    var response = await ChangeStatusAsync(id, model.Status);
                    if (!response.Success) return response;
                }

                if (!string.IsNullOrEmpty(model.StartTime))
                {
                    var response = await ChangeStartTimeAsync(id, model.StartTime);
                    if (!response.Success) return response;
                }

                if (!string.IsNullOrEmpty(model.EndTime))
                {
                    var response = await ChangeEndTimeAsync(id, model.EndTime);
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

        private async Task<Response> CreateWithAssignmentAsync(LearningProcessDto model)
        {
            var assignmentModel = await _unit.Assignments
                                .Include(a => a.CourseContent)
                                .FirstOrDefaultAsync(a => a.AssignmentId == model.AssignmentId);

            if (assignmentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            if (assignmentModel.CourseContent.Type != (int)CourseContentTypeEnum.Normal)
            {
                return new Response
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't create process with this assignment",
                    Success = false
                };
            }


            if (assignmentModel.NoNum == 1 && assignmentModel.CourseContent.NoNum == 1)
            {
                var processes = _unit.LearningProcesses.Find(p => p.EnrollId == model.EnrollId && p.AssignmentId == model.AssignmentId)
                                                       .ToList();



                if (processes != null && processes.Count > 0)
                {
                    var achieveProcesses = processes.Where(p => p.Status == (int)ProcessStatusEnum.Completed || p.Status == (int)ProcessStatusEnum.Achieved)
                                                    .ToList();


                    foreach (var process in achieveProcesses)
                    {
                        if (process.Status == (int)ProcessStatusEnum.Completed)
                        {
                            process.Status = (int)ProcessStatusEnum.Achieved;
                        }
                    }
                }
            }
            else
            {
                var processes = _unit.LearningProcesses.Find(p => p.EnrollId == model.EnrollId && p.AssignmentId == model.AssignmentId)
                                                       .ToList();

                if (processes != null && processes.Count > 0)
                {
                    var achieveProcesses = processes.Where(p => p.Status == (int)ProcessStatusEnum.Completed || p.Status == (int)ProcessStatusEnum.Achieved)
                                                    .ToList();

                    foreach (var process in achieveProcesses)
                    {
                        if (process.Status == (int)ProcessStatusEnum.Completed)
                        {
                            process.Status = (int)ProcessStatusEnum.Achieved;
                        }
                    }
                }
                else
                {
                    var previousCourseContent = await _unit.CourseContents.GetPreviousAsync(assignmentModel.CourseContent);

                    if (previousCourseContent != null && previousCourseContent.Type != 1)
                    {
                        if (previousCourseContent.Examination == null)
                        {
                            return new Response()
                            {
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Message = "Course content isn't set examinations",
                                Success = false
                            };
                        }

                        var isExistProcess = _unit.LearningProcesses
                                                    .IsExist(p => p.EnrollId == model.EnrollId &&
                                                                  p.ExamId == previousCourseContent.Examination.ExamId &&
                                                                  p.Status == (int)ProcessStatusEnum.Completed);
                        if (!isExistProcess)
                        {
                            return new Response()
                            {
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Message = "Please complete the previous assignment to continue with this exercise",
                                Success = false
                            };
                        }
                    }
                    else
                    {
                        Assignment? previousAssignment = await _unit.Assignments.GetPreviousAssignmentAsync(assignmentModel.AssignmentId);
                        if (previousAssignment == null)
                        {
                            return new Response()
                            {
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Message = "Create process failed",
                                Success = false
                            };
                        }

                        var isQualified = _unit.LearningProcesses
                                               .IsExist(p => p.EnrollId == model.EnrollId &&
                                                             p.AssignmentId == previousAssignment.AssignmentId &&
                                                             (p.Status == (int)ProcessStatusEnum.Achieved || p.Status == (int)ProcessStatusEnum.Completed));

                        if (!isQualified)
                        {
                            return new Response()
                            {
                                StatusCode = System.Net.HttpStatusCode.BadRequest,
                                Message = "Please complete the previous assignment to continue with this exercise",
                                Success = false
                            };
                        }
                    }
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        private async Task<Response> CreateWithExamAsync(LearningProcessDto model)
        {
            var examModel = await _unit.Examinations.Include(e => e.CourseContent).FirstOrDefaultAsync(a => a.ExamId == model.ExamId);

            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any examinations",
                    Success = false
                };
            }

            if (examModel.CourseContent.NoNum == 1)
            {
                var isExistProcess = _unit.LearningProcesses.IsExist(p => p.EnrollId == model.EnrollId && p.ExamId == examModel.ExamId);
                if (isExistProcess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "You cannot do it again",
                        Success = false
                    };
                }
            }
            else
            {
                var isExistProcessModel = _unit.LearningProcesses.IsExist(p => p.EnrollId == model.EnrollId && p.ExamId == examModel.ExamId);
                if (isExistProcessModel)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "You cannot do it again",
                        Success = false
                    };
                }

                var previousCourseContent = await _unit.CourseContents.GetPreviousAsync(examModel.CourseContent);
                if (previousCourseContent == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any course content",
                        Success = false
                    };
                }


                if (previousCourseContent.Type == (int)CourseContentTypeEnum.Normal)
                {
                    var assignment = _unit.Assignments
                                        .Find(a => a.CourseContentId == previousCourseContent.ContentId)
                                        .OrderByDescending(p => p.NoNum)
                                        .FirstOrDefault();

                    if (assignment == null)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Can't find any assignments",
                            Success = false
                        };
                    }

                    var isExistProcess = _unit.LearningProcesses
                                            .IsExist(p => p.EnrollId == model.EnrollId &&
                                                          p.AssignmentId == assignment.AssignmentId &&
                                                          (p.Status == (int)ProcessStatusEnum.Achieved || p.Status == (int)ProcessStatusEnum.Completed));


                    // Todo: Fake to coding FE
                    //if (!isExistProcess)
                    //{
                    //    return new Response()
                    //    {
                    //        StatusCode = System.Net.HttpStatusCode.BadRequest,
                    //        Message = "Please complete the previous assignment to continue with this exercise",
                    //        Success = false
                    //    };
                    //}
                }
                else
                {
                    if (previousCourseContent.Examination == null)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Course content isn't set examinations",
                            Success = false
                        };
                    }

                    var isExistProcess = _unit.LearningProcesses
                                            .IsExist(p => p.EnrollId == model.EnrollId &&
                                                          p.ExamId == previousCourseContent.Examination.ExamId &&
                                                          p.Status == (int)ProcessStatusEnum.Completed);
                    if (!isExistProcess)
                    {
                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Please complete the previous assignment to continue with this exercise",
                            Success = false
                        };
                    }
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        private async Task<Response> HandleSubmitWithAssignmentAsync(LearningProcess processModel, LearningProcessDto model)
        {
            try
            {
                if (model.AssignmentRecords != null && model.AssignmentRecords.Count > 0)
                {
                    foreach (var answer in model.AssignmentRecords)
                    {
                        var response = await _assignmentRecordService.CreateAsync(answer);
                        if (!response.Success) return response;
                    }
                }
                // Todo: Handle submit
                var correctNum = _unit.AssignmentRecords.Find(a => a.LearningProcessId == processModel.ProcessId && a.IsCorrect).Count();
                var numQues = await _unit.AssignQues.GetNumberByAssignmentAsync(processModel.AssignmentId!.Value);

                bool isQualified = (correctNum * 100.0 / numQues) > processModel.Assignment!.AchievedPercentage;

                processModel.Status = isQualified ? (int)ProcessStatusEnum.Completed : (int)ProcessStatusEnum.NotAchieved;
                processModel.EndTime = DateTime.Now;

                await _unit.CompleteAsync();
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        private async Task<Response> HandleSubmitWithExamAsync(LearningProcess processModel, LearningProcessDto model)
        {
            try
            {
                if (model.ToeicRecords != null && model.ToeicRecords.Count > 0)
                {
                    foreach (var record in model.ToeicRecords)
                    {
                        var response = await _toeicRecordService.CreateAsync(record);
                        if (!response.Success) return response;
                    }
                }

                var examModel = await _unit.Examinations
                                     .Include(e => e.CourseContent)
                                     .FirstOrDefaultAsync(e => e.ExamId == model.ExamId);

                if (examModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any examinations",
                        Success = false
                    };
                }

                var scoreHisModel = _unit.ScoreHis.GetById(processModel.Enrollment.ScoreHisId!.Value);
                if (scoreHisModel == null)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any score history",
                        Success = false
                    };
                }

                var totalCorrectLcNum = _unit.ToeicRecords
                                             .Include(t => t.SubToeic)
                                             .Where(t => t.SubToeic.QuesNo <= 100 && t.IsCorrect)
                                             .Count();

                var totalCorrectRcNum = _unit.ToeicRecords
                                             .Include(t => t.SubToeic)
                                             .Where(t => t.SubToeic.QuesNo > 100 && t.IsCorrect)
                                             .Count();

                var lcConversion = _unit.ToeicConversion
                                      .Find(t => t.NumberCorrect == totalCorrectLcNum &&
                                                 t.Section == ToeicEnum.Listening.ToString())
                                      .FirstOrDefault();
                var rcConversion = _unit.ToeicConversion
                                      .Find(t => t.NumberCorrect == totalCorrectRcNum &&
                                                 t.Section == ToeicEnum.Reading.ToString())
                                      .FirstOrDefault();

                var totalPoint = lcConversion!.EstimatedScore + rcConversion!.EstimatedScore;

                if (examModel.CourseContent.Type == (int)CourseContentTypeEnum.Entrance)
                {
                    scoreHisModel.EntrancePoint = totalPoint;
                }

                if (examModel.CourseContent.Type == (int)CourseContentTypeEnum.Midterm)
                {
                    scoreHisModel.MidtermPoint = totalPoint;
                }

                if (examModel.CourseContent.Type == (int)CourseContentTypeEnum.Final)
                {
                    scoreHisModel.FinalPoint = totalPoint;
                }

                processModel.Status = (int)ProcessStatusEnum.Completed;
                processModel.EndTime = DateTime.Now;

                await _unit.CompleteAsync();

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = "",
                    Success = true
                };

            }
            catch (Exception ex)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        private async Task<LessonStatusEnum> IsStatusWithAssignmentAsync(Enrollment enroll, Assignment assignment, bool isSequential)
        {
            var isExistProcess = false;

            isExistProcess = _unit.LearningProcesses
                                  .IsExist(p => p.EnrollId == enroll.EnrollId &&
                                                p.AssignmentId == assignment.AssignmentId &&
                                                (p.Status == (int)ProcessStatusEnum.Completed ||
                                                p.Status == (int)ProcessStatusEnum.Achieved));

            if (isExistProcess) return LessonStatusEnum.Passed;

            isExistProcess = _unit.LearningProcesses
                                  .IsExist(p => p.EnrollId == enroll.EnrollId &&
                                                p.AssignmentId == assignment.AssignmentId &&
                                                p.Status == (int)ProcessStatusEnum.NotAchieved);

            if (isExistProcess) return LessonStatusEnum.Failed;

            if (!isSequential) return LessonStatusEnum.Open;

            isExistProcess = _unit.LearningProcesses
                                  .IsExist(p => p.EnrollId == enroll.EnrollId &&
                                                p.AssignmentId == assignment.AssignmentId &&
                                                p.Status == (int)ProcessStatusEnum.Ongoing);

            if (isExistProcess) return LessonStatusEnum.Open;

            var preCourseContent = await _unit.CourseContents.GetPreviousAsync(assignment.CourseContent);
            if (preCourseContent == null && assignment.NoNum == 1) return LessonStatusEnum.Open;

            if (preCourseContent == null || preCourseContent.Type == 1)
            {
                var previousAssignment = await _unit.Assignments.GetPreviousAssignmentAsync(assignment.AssignmentId);
                if (previousAssignment == null) return LessonStatusEnum.Locked;

                isExistProcess = _unit.LearningProcesses
                                      .IsExist(p => p.EnrollId == enroll.EnrollId &&
                                                    p.AssignmentId == previousAssignment.AssignmentId &&
                                                    (p.Status == (int)ProcessStatusEnum.Achieved ||
                                                    p.Status == (int)ProcessStatusEnum.Completed));

                if (isExistProcess) return LessonStatusEnum.Open;

                return LessonStatusEnum.Locked;
            }

            if (preCourseContent.Type != 1)
            {
                isExistProcess = _unit.LearningProcesses.IsExist(p => p.EnrollId == enroll.EnrollId && p.ExamId == preCourseContent.Examination.ExamId);
                if (isExistProcess) return LessonStatusEnum.Open;
            }

            return LessonStatusEnum.Locked;
        }

        private async Task<LessonStatusEnum> IsStatusWithExamAsync(Enrollment enroll, Examination exam, bool isSequential)
        {
            var isExist = _unit.LearningProcesses.IsExist(p => p.EnrollId == enroll.EnrollId && p.ExamId == exam.ExamId);
            if (isExist) return LessonStatusEnum.Passed;
            if (!isSequential) return LessonStatusEnum.Open;

            var preCourseContent = await _unit.CourseContents.GetPreviousAsync(exam.CourseContent);
            if (preCourseContent == null) return LessonStatusEnum.Open;

            if (preCourseContent.Type != 1)
            {
                isExist = _unit.LearningProcesses.IsExist(p => p.EnrollId == enroll.EnrollId && p.ExamId == preCourseContent.Examination.ExamId);
                if (isExist) return LessonStatusEnum.Open;

                return LessonStatusEnum.Locked;
            }

            var preAssignment = _unit.Assignments
                                     .Find(a => a.CourseContentId == preCourseContent.ContentId)
                                     .OrderByDescending(a => a.NoNum)
                                     .FirstOrDefault();

            if (preAssignment == null) return LessonStatusEnum.Locked;

            isExist = _unit.LearningProcesses
                           .IsExist(p => p.EnrollId == enroll.EnrollId &&
                                         p.AssignmentId == preAssignment.AssignmentId &&
                                         (p.Status == (int)ProcessStatusEnum.Completed ||
                                         p.Status == (int)ProcessStatusEnum.Achieved));

            if (isExist) return LessonStatusEnum.Open;

            return LessonStatusEnum.Locked;
        }
    }
}
