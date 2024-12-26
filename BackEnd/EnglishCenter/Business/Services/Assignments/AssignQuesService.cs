using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace EnglishCenter.Business.Services.Assignments
{
    public class AssignQuesService : IAssignQuesService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly IAssignmentRecordService _assignmentRecordService;

        public AssignQuesService(IUnitOfWork unit, IMapper mapper, IAssignmentRecordService assignmentRecordService)
        {
            _unit = unit;
            _mapper = mapper;
            _assignmentRecordService = assignmentRecordService;
        }

        public async Task<Response> ChangeAssignmentIdAsync(long id, long assignmentId)
        {
            var assignQuesModel = _unit.AssignQues.GetById(id);
            if (assignQuesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Assignment Question",
                    Success = false
                };
            }

            var isExistAssignment = _unit.Assignments.IsExist(a => a.AssignmentId == assignmentId);
            if (!isExistAssignment)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignQues.ChangeAssignmentIdAsync(assignQuesModel, assignmentId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
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

        public async Task<Response> ChangeQuesAsync(long id, int type, long quesId)
        {
            var assignQuesModel = _unit.AssignQues.GetById(id);
            if (assignQuesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any Assignment Question",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(QuesTypeEnum), type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid type",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignQues.ChangeQuesAsync(assignQuesModel, (QuesTypeEnum)type, quesId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
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

        public async Task<Response> ChangeNoNumAsync(long id, int noNum)
        {
            var assignModel = _unit.AssignQues.GetById(id);
            if (assignModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignment questions",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.AssignQues.ChangeNoNumAsync(assignModel, noNum);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't change no num",
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

        public async Task<Response> CreateAsync(AssignQueDto model)
        {
            var assignmentModel = _unit.Assignments
                                        .Include(a => a.AssignQues)
                                        .FirstOrDefault(a => a.AssignmentId == model.AssignmentId);

            if (assignmentModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            if (!Enum.IsDefined(typeof(QuesTypeEnum), model.Type))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Invalid Type",
                    Success = false
                };
            }

            var isSameQues = await _unit.AssignQues.IsSameAssignQuesAsync((QuesTypeEnum)model.Type, model.AssignmentId, model.QuesId);
            if (isSameQues)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "It is not possible to add the same assignment question",
                    Success = false
                };
            }

            var isExistQuesType = await _unit.AssignQues.IsExistQuesIdAsync((QuesTypeEnum)model.Type, model.QuesId);
            if (!isExistQuesType)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question",
                    Success = false
                };
            }

            var assignModel = _mapper.Map<AssignQue>(model);
            var currentMaxNum = assignmentModel.AssignQues.Count > 0 ? assignmentModel.AssignQues.Max(c => c.NoNum) : 0;

            assignModel.NoNum = currentMaxNum + 1;

            _unit.AssignQues.Add(assignModel);

            var expectedTime = await _unit.AssignQues.GetTimeQuesAsync(assignModel);
            assignmentModel.ExpectedTime = assignmentModel.ExpectedTime.Add(expectedTime.ToTimeSpan());

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
            var assignQuesModel = _unit.AssignQues
                                        .Include(a => a.Assignment)
                                        .Include(a => a.AssignmentRecords)
                                        .FirstOrDefault(a => a.AssignQuesId == id);

            if (assignQuesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignment question",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                var expectedTime = await _unit.AssignQues.GetTimeQuesAsync(assignQuesModel);
                assignQuesModel.Assignment.ExpectedTime = TimeOnly.FromTimeSpan(assignQuesModel.Assignment.ExpectedTime - expectedTime);

                var assignmentRecords = assignQuesModel.AssignmentRecords.Select(a => a.RecordId).ToList();
                foreach (var assignment in assignmentRecords)
                {
                    await _assignmentRecordService.DeleteAsync(assignment);
                }

                int i = 1;
                var otherAssignQues = _unit.AssignQues
                                           .Find(a => a.AssignmentId == assignQuesModel.AssignmentId && a.NoNum != assignQuesModel.NoNum)
                                           .OrderBy(a => a.NoNum);

                foreach (var assignQue in otherAssignQues)
                {
                    assignQue.NoNum = i++;
                }

                _unit.AssignQues.Remove(assignQuesModel);

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

        public async Task<Response> GetAllAsync()
        {
            var models = _unit.AssignQues.GetAll();

            foreach (var model in models)
            {
                var isSuccess = await _unit.AssignQues.LoadQuestionAsync(model);
                if (!isSuccess)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Load question fail",
                        Success = false
                    };
                }
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignQueResDto>>(models.OrderBy(m => m.NoNum)),
                Success = true
            };
        }

        public async Task<Response> GetByAssignmentNormalAsync(long assignmentId)
        {
            var isExist = _unit.Assignments.IsExist(a => a.AssignmentId == assignmentId);
            if (!isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            var assignQues = await _unit.AssignQues.GetByAssignmentAsync(assignmentId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignQueResDto>>(assignQues?.OrderBy(s => s.Type)),
                Success = true
            };
        }

        public async Task<Response> GetByAssignmentAsync(long assignmentId)
        {
            var isExist = _unit.Assignments.IsExist(a => a.AssignmentId == assignmentId);
            if (!isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            var assignQues = await _unit.AssignQues.GetByAssignmentAsync(assignmentId);

            if (assignQues != null && assignQues.Count != 0)
            {
                assignQues = assignQues.GroupBy(model => model.Type)
                                       .Select(group => group.OrderBy(x => Guid.NewGuid()).ToList())
                                       .SelectMany(group => group)
                                       .OrderBy(a => a.Type)
                                       .ToList();
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignQueResDto>>(assignQues),
                Success = true
            };
        }

        public async Task<Response> GetAsync(long id)
        {
            var model = _unit.AssignQues.GetById(id);

            await _unit.AssignQues.LoadQuestionAsync(model);
            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignQueResDto>(model),
                Success = true
            };
        }

        public async Task<Response> GetResultAsync(long id)
        {
            var assignQues = _unit.AssignQues.GetById(id);

            await _unit.AssignQues.LoadQuestionAsync(assignQues);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignQueResDto>(assignQues),
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, AssignQueDto model)
        {
            var isSuccess = await _unit.AssignQues.UpdateAsync(id, model);
            if (!isSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
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

        public async Task<Response> GetAssignQuesByNoNumAsync(long assignmentId, int noNum)
        {
            var assignQue = _unit.AssignQues
                                        .Find(a => a.AssignmentId == assignmentId && a.NoNum == noNum)
                                        .FirstOrDefault();

            if (assignQue == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = null,
                    Success = true
                };
            }

            var isDone = await _unit.AssignQues.LoadQuestionWithoutAnswerAsync(assignQue);
            if (!isDone)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<AssignQueResDto>(assignQue),
                Success = true
            };
        }

        public async Task<Response> HandleCreateRandomWithLevelAsync(long assignmentId, List<RandomTypeWithLevelDto> randomModels)
        {
            var assignModel = _unit.Assignments.GetById(assignmentId);
            if (assignModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any assignments",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                foreach (var model in randomModels)
                {
                    var existQueIds = await GetExistQuesAsync(assignmentId, model.Type);

                    var selectedQuestions = new List<long>();

                    if (model.NumNormal != 0)
                    {
                        int level = 1;
                        var selectedQuesNormal = new List<long>();

                        while (selectedQuesNormal.Count < model.NumNormal && level > 0)
                        {
                            int remaining = model.NumNormal - selectedQuesNormal.Count;
                            var otherQueIds = await GetOtherQuesAsync(existQueIds, model.Type, level);
                            if (otherQueIds.Count > 0)
                            {
                                var random = new Random();
                                var randomQues = otherQueIds
                                                .Skip(random.Next(0, Math.Max(0, otherQueIds.Count - remaining)))
                                                .Take(remaining)
                                                .Where(quesId => !selectedQuestions.Contains(quesId))
                                                .ToList();

                                selectedQuesNormal.AddRange(randomQues);
                            }

                            level--;
                        }
                        existQueIds.AddRange(selectedQuesNormal);
                    }

                    if (model.NumIntermediate != 0)
                    {
                        int level = 2;
                        var selectedQuesIntermediate = new List<long>();

                        while (selectedQuesIntermediate.Count < model.NumIntermediate && level > 0)
                        {
                            int remaining = model.NumIntermediate - selectedQuesIntermediate.Count;
                            var otherQueIds = await GetOtherQuesAsync(existQueIds, model.Type, level);
                            if (otherQueIds.Count > 0)
                            {
                                var random = new Random();
                                var randomQues = otherQueIds
                                                .Skip(random.Next(0, Math.Max(0, otherQueIds.Count - remaining)))
                                                .Take(remaining)
                                                .Where(quesId => !selectedQuestions.Contains(quesId))
                                                .ToList();

                                selectedQuesIntermediate.AddRange(randomQues);
                            }

                            level--;
                        }

                        existQueIds.AddRange(selectedQuesIntermediate);
                    }

                    if (model.NumHard != 0)
                    {
                        int level = 3;
                        var selectedQuesHard = new List<long>();

                        while (selectedQuesHard.Count < model.NumHard && level > 0)
                        {
                            int remaining = model.NumHard - selectedQuesHard.Count;
                            var otherQueIds = await GetOtherQuesAsync(existQueIds, model.Type, level);
                            if (otherQueIds.Count > 0)
                            {
                                var random = new Random();
                                var randomQues = otherQueIds
                                                .Skip(random.Next(0, Math.Max(0, otherQueIds.Count - remaining)))
                                                .Take(remaining)
                                                .Where(quesId => !selectedQuestions.Contains(quesId))
                                                .ToList();

                                selectedQuesHard.AddRange(randomQues);
                            }

                            level--;
                        }

                        existQueIds.AddRange(selectedQuesHard);
                    }

                    if (model.NumVeryHard != 0)
                    {
                        int level = 4;
                        var selectedQuesVeryHard = new List<long>();

                        while (selectedQuesVeryHard.Count < model.NumVeryHard && level > 0)
                        {
                            int remaining = model.NumVeryHard - selectedQuesVeryHard.Count;
                            var otherQueIds = await GetOtherQuesAsync(existQueIds, model.Type, level);
                            if (otherQueIds.Count > 0)
                            {
                                var random = new Random();
                                var randomQues = otherQueIds
                                                .Skip(random.Next(0, Math.Max(0, otherQueIds.Count - remaining)))
                                                .Take(remaining)
                                                .Where(quesId => !selectedQuestions.Contains(quesId))
                                                .ToList();

                                selectedQuesVeryHard.AddRange(randomQues);
                            }

                            level--;
                        }

                        existQueIds.AddRange(selectedQuesVeryHard);
                    }

                    var listDtos = existQueIds.Select(s => new AssignQueDto()
                    {
                        AssignmentId = assignmentId,
                        QuesId = s,
                        Type = model.Type
                    })
                                                .ToList();

                    foreach (var item in listDtos)
                    {
                        var createRes = await CreateAsync(item);
                        if (!createRes.Success) return createRes;
                    }

                }

                TimeOnly finalTime;
                TimeOnly originalTime = assignModel.ExpectedTime;

                if (originalTime.Second > 0 || originalTime.Millisecond > 0)
                {
                    finalTime = originalTime.Add(TimeSpan.FromMinutes(1))
                                            .Add(TimeSpan.FromSeconds(-originalTime.Second))
                                            .Add(TimeSpan.FromMilliseconds(-originalTime.Millisecond));
                }
                else
                {
                    finalTime = originalTime;
                }

                assignModel.Time = finalTime;

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

        private Task<List<long>> GetOtherQuesAsync(HashSet<long> existQueIds, int type, int level)
        {
            var result = new List<long>();

            if (type == (int)QuesTypeEnum.Image)
            {
                result = _unit.QuesLcImages
                              .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                              .Select(s => s.QuesId)
                              .ToList();
            }

            if (type == (int)QuesTypeEnum.Audio)
            {
                result = _unit.QuesLcAudios
                              .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                              .Select(s => s.QuesId)
                              .ToList();
            }

            if (type == (int)QuesTypeEnum.Conversation)
            {
                result = _unit.QuesLcCons
                               .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                               .Select(s => s.QuesId)
                               .ToList();
            }

            if (type == (int)QuesTypeEnum.Sentence)
            {
                result = _unit.QuesRcSentences
                              .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                              .Select(s => s.QuesId)
                              .ToList();
            }

            if (type == (int)QuesTypeEnum.Single)
            {
                result = _unit.QuesRcSingles
                               .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                               .Select(s => s.QuesId)
                               .ToList();
            }

            if (type == (int)QuesTypeEnum.Double)
            {
                result = _unit.QuesRcDoubles
                               .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                               .Select(s => s.QuesId)
                               .ToList();
            }

            if (type == (int)QuesTypeEnum.Triple)
            {
                result = _unit.QuesRcTriples
                              .Find(s => !existQueIds.Contains(s.QuesId) && s.Level == level)
                              .Select(s => s.QuesId)
                              .ToList();
            }

            return Task.FromResult(result);
        }

        private Task<HashSet<long>> GetExistQuesAsync(long assignmentId, int type)
        {
            var existQueIds = new HashSet<long>();

            if (type == (int)QuesTypeEnum.Image)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Image)
                                   .Where(s => s.ImageQuesId.HasValue)
                                   .Select(s => s.ImageQuesId!.Value)
                                   .ToHashSet();
            }

            if (type == (int)QuesTypeEnum.Audio)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Audio)
                                   .Where(s => s.AudioQuesId.HasValue)
                                   .Select(s => s.AudioQuesId!.Value)
                                   .ToHashSet();
            }

            if (type == (int)QuesTypeEnum.Conversation)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Conversation)
                                   .Where(s => s.ConversationQuesId.HasValue)
                                   .Select(s => s.ConversationQuesId!.Value)
                                   .ToHashSet();
            }

            if (type == (int)QuesTypeEnum.Sentence)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Sentence)
                                   .Where(s => s.SentenceQuesId.HasValue)
                                   .Select(s => s.SentenceQuesId!.Value)
                                   .ToHashSet();
            }

            if (type == (int)QuesTypeEnum.Single)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Single)
                                   .Where(s => s.SingleQuesId.HasValue)
                                   .Select(s => s.SingleQuesId!.Value)
                                   .ToHashSet();
            }

            if (type == (int)QuesTypeEnum.Double)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Double)
                                    .Where(s => s.DoubleQuesId.HasValue)
                                   .Select(s => s.DoubleQuesId!.Value)
                                   .ToHashSet();
            }

            if (type == (int)QuesTypeEnum.Triple)
            {
                existQueIds = _unit.AssignQues
                                   .Find(s => s.AssignmentId == assignmentId && s.Type == (int)QuesTypeEnum.Triple)
                                   .Where(s => s.TripleQuesId.HasValue)
                                   .Select(s => s.TripleQuesId!.Value)
                                   .ToHashSet();
            }

            return Task.FromResult(existQueIds);
        }

        private Task<int> GetNumQuesAsync(int type)
        {
            if (type == (int)QuesTypeEnum.Image)
            {
                return Task.FromResult(_unit.QuesLcImages.GetAll().Count());
            }

            if (type == (int)QuesTypeEnum.Audio)
            {
                return Task.FromResult(_unit.QuesLcAudios.GetAll().Count());
            }

            if (type == (int)QuesTypeEnum.Conversation)
            {
                return Task.FromResult(_unit.QuesLcCons.GetAll().Count());
            }

            if (type == (int)QuesTypeEnum.Sentence)
            {
                return Task.FromResult(_unit.QuesRcSentences.GetAll().Count());
            }

            if (type == (int)QuesTypeEnum.Single)
            {
                return Task.FromResult(_unit.QuesRcSingles.GetAll().Count());
            }

            if (type == (int)QuesTypeEnum.Double)
            {
                return Task.FromResult(_unit.QuesRcDoubles.GetAll().Count());
            }

            return Task.FromResult(_unit.QuesRcTriples.GetAll().Count());
        }

        public async Task<Response> GetMaxNumQuesAsync()
        {
            var listResult = new List<MaxNumQuesResDto>();
            foreach (QuesTypeEnum value in Enum.GetValues(typeof(QuesTypeEnum)))
            {
                var result = new MaxNumQuesResDto()
                {
                    Type = (int)value,
                    TypeName = value.ToString(),
                    MaxNum = await GetNumQuesAsync((int)value)
                };

                listResult.Add(result);
            }


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = listResult,
                Success = true
            };
        }

        public async Task<Response> HandleCreateListAsync(long assignmentId, List<AssignQueDto> models)
        {
            await _unit.BeginTransAsync();
            try
            {
                foreach (var model in models)
                {
                    var createRes = await CreateAsync(model);
                    if (!createRes.Success) return createRes;
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

        public async Task<Response> GetByProcessesAsync(long processId)
        {
            var isExist = _unit.LearningProcesses.IsExist(a => a.ProcessId == processId && a.Status != (int)ProcessStatusEnum.Ongoing);

            if (!isExist)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any processes",
                    Success = false
                };
            }

            var assignQues = await _unit.AssignQues.GetByProcessAsync(processId);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<AssignQueResDto>>(assignQues),
                Success = true
            };
        }
    }
}
