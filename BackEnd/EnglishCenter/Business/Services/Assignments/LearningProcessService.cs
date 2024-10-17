using System.Linq;
using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Assignments
{
    public class LearningProcessService : ILearningProcessService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IAnswerRecordService _answerService;

        public LearningProcessService(IUnitOfWork unit, IMapper mapper, IAnswerRecordService answerService)
        {
            _unit = unit;
            _mapper = mapper;
            _answerService = answerService;
        }

        public async Task<Response> ChangeEndTimeAsync(long id, string dateTime)
        {
            var processModel = _unit.LearningProcesses.GetById(id);
            if(processModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            if(!DateTime.TryParse(dateTime, out DateTime time))
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
            if(!Enum.IsDefined(typeof(ProcessStatusEnum), status))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Status is not valid",
                    Success = false
                };
            }

            var processModel = _unit.LearningProcesses.GetById(id);
            if(processModel == null)
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

            if(enrollModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any enrollments",
                    Success = false
                };
            }

            if(enrollModel.StatusId != (int) EnrollEnum.Ongoing)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Class has not started yet",
                    Success = false
                };
            }

            var assignmentModel = await _unit.Assignments
                                    .Include(a => a.CourseContent)
                                    .FirstOrDefaultAsync(a => a.AssignmentId == model.AssignmentId);

            if(assignmentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            var courseModel = _unit.Courses.GetById(enrollModel.Class.CourseId);
            var entity = new LearningProcess();

            if (courseModel.IsSequential)
            {
                if(assignmentModel.NoNum == 1 && assignmentModel.CourseContent.NoNum == 1)
                {
                    var processes = _unit.LearningProcesses.Find(p => p.EnrollId == model.EnrollId && p.AssignmentId == model.AssignmentId)
                                                           .ToList();
                    
                    if(processes != null && processes.Count > 0)
                    {
                        var achieveProcesses = processes.Where(p => p.Status == (int) ProcessStatusEnum.Completed || p.Status == (int) ProcessStatusEnum.Achieved)
                                                        .ToList();

                        foreach(var process in achieveProcesses)
                        {
                            if(process.Status == (int)ProcessStatusEnum.Completed)
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

                    if(processes != null && processes.Count > 0)
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
                        Assignment? previousAssignment = await _unit.Assignments.GetPreviousAssignmentAsync(assignmentModel.AssignmentId);

                        if(previousAssignment == null)
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
            else
            {
                var isExist = _unit.LearningProcesses.IsExist(c => c.EnrollId == model.EnrollId && c.AssignmentId == model.AssignmentId);
                if (isExist)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Create process failed",
                        Success = false
                    };
                }
            }

            entity = _mapper.Map<LearningProcess>(model);
            _unit.LearningProcesses.Add(entity);

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = entity,
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var processModel = _unit.LearningProcesses
                                    .Include(a => a.Assignment)
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

            if(processModel.Status == (int) ProcessStatusEnum.NotAchieved || processModel.Status == (int)ProcessStatusEnum.Ongoing)
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
                var courseContentModel = _unit.CourseContents.GetById(processModel.Assignment.CourseContentId);

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
                Message = processes,
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
                Message = process,
                Success = true
            });
        }

        public async Task<Response> HandleSubmitProcessAsync(long id, LearningProcessDto model)
        {
            var processModel = _unit.LearningProcesses
                                    .Include(p => p.Assignment)
                                    .FirstOrDefault(p => p.ProcessId == id);

            if(processModel == null)
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
                if (model.Answers != null && model.Answers.Count > 0)
                {
                    foreach (var answer in model.Answers)
                    {
                        var response = await _answerService.CreateAsync(answer);
                        if (!response.Success) return response;
                    }
                }

                var correctNum = _unit.AnswerRecords.Find(a => a.LearningProcessId == id && a.IsCorrect == true).Count();
                var numQues = await _unit.AssignQues.GetNumberByAssignmentAsync(processModel.AssignmentId);

                bool isQualified = (correctNum * 100.0 / numQues) > processModel.Assignment.AchievedPercentage;

                processModel.Status = isQualified ? (int)ProcessStatusEnum.Completed : (int)ProcessStatusEnum.NotAchieved;
                processModel.EndTime = DateTime.Now;


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

        public async Task<Response> UpdateAsync(long id, LearningProcessDto model)
        {
            var processModel = _unit.LearningProcesses.GetById(id);

            if(processModel == null)
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
