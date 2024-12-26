using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Helpers;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class RandomQueToeicService : IRandomQueToeicService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RandomQueToeicService(IMapper mapper, IUnitOfWork unit, IWebHostEnvironment webHostEnvironment)
        {
            _mapper = mapper;
            _unit = unit;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Response> ChangeHomeworkAsync(long id, long homeworkId)
        {
            var ranQueModel = _unit.RandomQues.GetById(id);
            if (ranQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any random questions",
                    Success = false
                };
            }

            var hwModel = _unit.Homework.GetById(homeworkId);
            if (hwModel == null)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any homework",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.RandomQues.ChangeHomeworkAsync(ranQueModel, homeworkId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change homework failed",
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

        public async Task<Response> ChangeQuesToeicAsync(long id, long quesId)
        {
            var ranQueModel = _unit.RandomQues.GetById(id);
            if (ranQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any random questions",
                    Success = false
                };
            }

            var quesModel = _unit.QuesToeic.GetById(quesId);
            if (quesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question toeic",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.RandomQues.ChangeQuesToeicAsync(ranQueModel, quesId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change question toeic failed",
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

        public async Task<Response> ChangeRoadMapAsync(long id, long examId)
        {
            var ranQueModel = _unit.RandomQues.GetById(id);
            if (ranQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any random questions",
                    Success = false
                };
            }

            var roadMapExamModel = _unit.RoadMapExams.GetById(examId);
            if (roadMapExamModel == null)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any road map exams",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.RandomQues.ChangeRoadMapExamAsync(ranQueModel, examId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change road map exam failed",
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

        public async Task<Response> CreateAsync(RandomQuesToeicDto model, bool isAddTime = true)
        {
            if (!(model.RoadMapExamId.HasValue ^ model.HomeworkId.HasValue))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Question only belong to homework or exam",
                    Success = false
                };
            }

            var quesModel = await _unit.QuesToeic
                                       .Include(q => q.SubToeicList)
                                       .FirstOrDefaultAsync(q => q.QuesId == model.QuesToeicId);

            if (quesModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any question toeic",
                    Success = false
                };
            }

            if (model.HomeworkId.HasValue)
            {
                var hwModel = _unit.Homework.GetById(model.HomeworkId.Value);
                if (hwModel == null)
                {

                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any homework",
                        Success = false
                    };
                }

                if (hwModel.Type == 1)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Homework is not valid type",
                        Success = false
                    };
                }

                if (quesModel.Part <= (int)PartEnum.Part4)
                {
                    var durationAudio = await VideoHelper.GetDurationAsync(Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio ?? ""));
                    hwModel.ExpectedTime = hwModel.ExpectedTime.Add(durationAudio);
                }
                else
                {
                    hwModel.ExpectedTime = hwModel.ExpectedTime.Add(TimeSpan.FromSeconds(45 * quesModel.SubToeicList.Count));
                }

                var randomQues = _mapper.Map<RandomQuesToeic>(model);
                _unit.RandomQues.Add(randomQues);
            }
            if (model.RoadMapExamId.HasValue)
            {
                var roadMapExamModel = _unit.RoadMapExams.GetById(model.RoadMapExamId.Value);
                if (roadMapExamModel == null)
                {

                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Can't find any road map exams",
                        Success = false
                    };
                }

                var numberListening = await _unit.RandomQues.GetTotalNumberQuesAsync(roadMapExamModel.RoadMapExamId);
                var numberReading = await _unit.RandomQues.GetTotalNumberQuesAsync(roadMapExamModel.RoadMapExamId, false);

                var timeQues = TimeSpan.Zero;

                if (quesModel.Part <= (int)PartEnum.Part4)
                {
                    numberListening++;
                    timeQues = await VideoHelper.GetDurationAsync(Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio ?? ""));
                }
                else
                {
                    numberReading++;
                    timeQues += TimeSpan.FromSeconds(45 * quesModel.SubToeicList.Count);
                }

                var listeningScore = _unit.ToeicConversion
                                           .Find(c => c.NumberCorrect == numberListening && c.Section == ToeicEnum.Listening.ToString())
                                           .First()
                                           .EstimatedScore;
                var readingScore = _unit.ToeicConversion
                                        .Find(c => c.NumberCorrect == numberReading && c.Section == ToeicEnum.Reading.ToString())
                                        .First()
                                        .EstimatedScore;

                roadMapExamModel.Point = listeningScore + readingScore;

                if (isAddTime)
                {
                    roadMapExamModel.TimeMinutes = Math.Round(timeQues.Add(TimeSpan.FromMinutes(roadMapExamModel.TimeMinutes)).TotalMinutes, 2);
                }

                var randomQues = _mapper.Map<RandomQuesToeic>(model);
                _unit.RandomQues.Add(randomQues);
            }

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
            var ranQueModel = _unit.RandomQues.GetById(id);
            if (ranQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any random questions",
                    Success = false
                };
            }

            var quesModel = _unit.QuesToeic.Include(q => q.SubToeicList).FirstOrDefault(q => q.QuesId == ranQueModel.QuesToeicId);
            if (quesModel == null)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            if (ranQueModel.HomeworkId.HasValue)
            {
                var homeworkModel = _unit.Homework.GetById(ranQueModel.HomeworkId.Value);

                if (quesModel.Part <= (int)PartEnum.Part4)
                {
                    var durationAudio = await VideoHelper.GetDurationAsync(Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio ?? ""));
                    homeworkModel.ExpectedTime = TimeOnly.FromTimeSpan(homeworkModel.ExpectedTime.ToTimeSpan().Subtract(durationAudio));
                }
                else
                {
                    var time = TimeSpan.FromSeconds(45 * quesModel.SubToeicList.Count);
                    homeworkModel.ExpectedTime = TimeOnly.FromTimeSpan(homeworkModel.ExpectedTime.ToTimeSpan().Subtract(time));
                }

                var totalSeconds = homeworkModel.ExpectedTime.ToTimeSpan().TotalSeconds;
                var roundedSeconds = (int)Math.Round(totalSeconds);

                homeworkModel.Time = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(roundedSeconds));
            }

            _unit.RandomQues.Remove(ranQueModel);
            await _unit.CompleteAsync();

            if (ranQueModel.RoadMapExamId.HasValue)
            {
                var roadMapExam = _unit.RoadMapExams.GetById(ranQueModel.RoadMapExamId.Value);

                var numberListening = await _unit.RandomQues.GetTotalNumberQuesAsync(roadMapExam.RoadMapExamId);
                var numberReading = await _unit.RandomQues.GetTotalNumberQuesAsync(roadMapExam.RoadMapExamId, false);

                var timeQues = TimeSpan.Zero;

                if (quesModel.Part <= (int)PartEnum.Part4)
                {
                    numberListening--;
                    timeQues = await VideoHelper.GetDurationAsync(Path.Combine(_webHostEnvironment.WebRootPath, quesModel.Audio ?? ""));
                }
                else
                {
                    numberReading--;
                    timeQues += TimeSpan.FromSeconds(45 * quesModel.SubToeicList.Count);
                }

                var listeningScore = _unit.ToeicConversion
                                           .Find(c => c.NumberCorrect == numberListening && c.Section == ToeicEnum.Listening.ToString())
                                           .FirstOrDefault();
                var readingScore = _unit.ToeicConversion
                                        .Find(c => c.NumberCorrect == numberReading && c.Section == ToeicEnum.Reading.ToString())
                                        .FirstOrDefault();

                var lcScore = listeningScore == null ? 0 : listeningScore.EstimatedScore;
                var rcScore = readingScore == null ? 0 : readingScore.EstimatedScore;
                roadMapExam.Point = lcScore + rcScore;
                roadMapExam.TimeMinutes = Math.Round(TimeSpan.FromMinutes(roadMapExam.TimeMinutes).Subtract(timeQues).TotalMinutes, 2);
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> DeleteRmAsync(long roadMapId, long quesId)
        {
            var ranModel = _unit.RandomQues.Find(r => r.RoadMapExamId == roadMapId && r.QuesToeicId == quesId).FirstOrDefault();
            if (ranModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var deleteRes = await DeleteAsync(ranModel.Id);
            return deleteRes;
        }

        public async Task<Response> DeleteHwAsync(long homeworkId, long quesId)
        {
            var ranModel = _unit.RandomQues.Find(r => r.HomeworkId == homeworkId && r.QuesToeicId == quesId).FirstOrDefault();
            if (ranModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any questions",
                    Success = false
                };
            }

            var deleteRes = await DeleteAsync(ranModel.Id);
            return deleteRes;
        }

        public Task<Response> GetAllAsync()
        {
            var models = _unit.RandomQues.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<RandomQuesToeicDto>>(models),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var model = _unit.RandomQues.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<RandomQuesToeicDto>(model),
                Success = true
            });
        }

        public async Task<Response> GetByDefaultHwAsync(long homeworkId)
        {
            var quesModels = await _unit.RandomQues
                                         .Include(r => r.QuesToeic)
                                         .Where(r => r.HomeworkId == homeworkId)
                                         .Select(r => r.QuesToeic)
                                         .ToListAsync();

            foreach (var ran in quesModels)
            {
                await _unit.QuesToeic.LoadSubQuesWithAnswer(ran);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesToeicResDto>>(quesModels.OrderBy(q => q.Part)),
                Success = true
            };
        }

        public async Task<Response> GetByHomeworkAsync(long homeworkId)
        {
            var quesModels = await _unit.RandomQues
                                          .Include(r => r.QuesToeic)
                                          .Where(r => r.HomeworkId == homeworkId)
                                          .Select(r => r.QuesToeic)
                                          .ToListAsync();

            foreach (var ran in quesModels)
            {
                await _unit.QuesToeic.LoadSubQuesAsync(ran);
            }

            var newResModels = quesModels.GroupBy(q => q.Part)
                                         .Select(g => g.OrderBy(x => Guid.NewGuid()).ToList())
                                         .SelectMany(g => g)
                                         .ToList();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesToeicResDto>>(newResModels.OrderBy(q => q.Part)),
                Success = true
            };
        }

        public async Task<Response> GetByRoadMapExamAsync(long examId)
        {
            var quesModels = await _unit.RandomQues
                                          .Include(r => r.QuesToeic)
                                          .Where(r => r.RoadMapExamId == examId)
                                          .Select(r => r.QuesToeic)
                                          .ToListAsync();

            foreach (var ran in quesModels)
            {
                await _unit.QuesToeic.LoadSubQuesAsync(ran);
            }

            var newResModels = quesModels.GroupBy(q => q.Part)
                                         .Select(g => g.OrderBy(x => Guid.NewGuid()).ToList())
                                         .SelectMany(g => g)
                                         .ToList();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesToeicResDto>>(newResModels.OrderBy(q => q.Part)),
                Success = true
            };
        }

        public async Task<Response> GetNumberQuesByHwAsync(long homeworkId)
        {
            var numQuesToeic = new NumQuesToeicResDto()
            {
                Part1 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part1),
                Part2 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part2),
                Part3 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part3),
                Part4 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part4),
                Part5 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part5),
                Part6 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part6),
                Part7 = await _unit.RandomQues.GetNumberByPartHwAsync(homeworkId, (int)PartEnum.Part7),
            };

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = numQuesToeic,
                Success = true
            };
        }

        public async Task<Response> GetNumberQuesByRmAsync(long roadMapExamId)
        {
            var numQuesToeic = new NumQuesToeicResDto()
            {
                Part1 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part1),
                Part2 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part2),
                Part3 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part3),
                Part4 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part4),
                Part5 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part5),
                Part6 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part6),
                Part7 = await _unit.RandomQues.GetNumberByPartRmAsync(roadMapExamId, (int)PartEnum.Part7),
            };

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = numQuesToeic,
                Success = true
            };
        }

        public async Task<Response> UpdateAsync(long id, RandomQuesToeicDto model)
        {
            var ranQueModel = _unit.RandomQues.GetById(id);
            if (ranQueModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any random questions",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();

            try
            {
                if (model.RoadMapExamId.HasValue && ranQueModel.RoadMapExamId != model.RoadMapExamId.Value)
                {
                    var response = await ChangeRoadMapAsync(id, model.RoadMapExamId.Value);
                    if (!response.Success) return response;
                }
                if (model.HomeworkId.HasValue && ranQueModel.HomeworkId != model.HomeworkId.Value)
                {
                    var response = await ChangeHomeworkAsync(id, model.HomeworkId.Value);
                    if (!response.Success) return response;
                }

                if (model.QuesToeicId != model.QuesToeicId)
                {
                    var response = await ChangeQuesToeicAsync(id, model.QuesToeicId);
                    if (!response.Success) return response;
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

        public async Task<Response> HandleCreateRmWithLevelAsync(RandomPartDto model)
        {
            if (model.Level < 1 || model.Level > 4)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Level is invalid",
                    Success = false
                };
            }

            if (model.Number < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Number must be greater than 0",
                    Success = false
                };
            }

            if (!model.RoadMapExamId.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Road map exam id is required",
                    Success = false
                };
            }

            var roadMapModel = _unit.RoadMapExams.GetById(model.RoadMapExamId.Value);
            if (roadMapModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any road map exams",
                    Success = false
                };
            }

            var existQueIds = _unit.RandomQues
                                   .Find(r => r.RoadMapExamId == model.RoadMapExamId)
                                   .Select(r => r.QuesToeicId)
                                   .ToHashSet();

            var numberQuePart = _unit.RandomQues
                                     .Include(r => r.QuesToeic)
                                     .Where(r => r.RoadMapExamId == model.RoadMapExamId && r.QuesToeic.Part == model.Part)
                                     .Count();

            if (model.Part == (int)PartEnum.Part1 && numberQuePart + model.Number > 6)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 1 ranges from 1-6 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part2 && numberQuePart + model.Number > 25)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 2 ranges from 1-25 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part3 && numberQuePart + model.Number > 13)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 3 ranges from 1-13 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part4 && numberQuePart + model.Number > 10)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 4 ranges from 1-10 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part5 && numberQuePart + model.Number > 30)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 5 ranges from 1-30 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part6 && numberQuePart + model.Number > 4)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 6 ranges from 1-4 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part7 && numberQuePart + model.Number > 15)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 7 ranges from 1-15 questions",
                    Success = false
                };
            }


            var selectedQuestions = new List<long>();

            while (selectedQuestions.Count < model.Number && model.Level > 0)
            {
                int remaining = model.Number - selectedQuestions.Count;

                int totalQuestions = _unit.QuesToeic
                                          .Find(q => q.Part == model.Part && q.Level == model.Level && !existQueIds.Contains(q.QuesId))
                                          .Count();

                if (totalQuestions > 0)
                {
                    var random = new Random();
                    var randomQuestions = _unit.QuesToeic
                                               .Find(q => q.Part == model.Part && q.Level == model.Level && !existQueIds.Contains(q.QuesId))
                                               .Skip(random.Next(0, Math.Max(0, totalQuestions - remaining)))
                                               .Take(remaining)
                                               .Select(q => q.QuesId)
                                               .Where(quesId => !selectedQuestions.Contains(quesId))
                                               .ToList();

                    selectedQuestions.AddRange(randomQuestions);
                }

                model.Level--;
            }

            var newRandomQues = selectedQuestions.Select(qId => new RandomQuesToeicDto
            {
                RoadMapExamId = model.RoadMapExamId,
                QuesToeicId = qId
            }).ToList();

            foreach (var ran in newRandomQues)
            {
                var response = await CreateAsync(ran);
                if (!response.Success) return response;
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> HandleCreateHwWithLevelAsync(RandomPartDto model)
        {
            if (model.Level < 1 || model.Level > 4)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Level is invalid",
                    Success = false
                };
            }

            if (model.Number < 0)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Number must be greater than 0",
                    Success = false
                };
            }

            if (!model.HomeworkId.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Homework id is required",
                    Success = false
                };
            }

            var homeworkModel = _unit.Homework.GetById(model.HomeworkId.Value);
            if (homeworkModel == null || (homeworkModel != null && homeworkModel.Type == 1))
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Homework is invalid",
                    Success = false
                };
            }

            var existQueIds = _unit.RandomQues
                                   .Find(r => r.HomeworkId == model.HomeworkId)
                                   .Select(r => r.QuesToeicId)
                                   .ToHashSet();

            var numberQuePart = _unit.RandomQues
                                    .Include(r => r.QuesToeic)
                                    .Where(r => r.HomeworkId == model.HomeworkId && r.QuesToeic.Part == model.Part)
                                    .Count();


            if (model.Part == (int)PartEnum.Part1 && numberQuePart + model.Number > 6)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 1 ranges from 1-6 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part2 && numberQuePart + model.Number > 25)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 2 ranges from 1-25 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part3 && numberQuePart + model.Number > 13)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 3 ranges from 1-13 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part4 && numberQuePart + model.Number > 10)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 4 ranges from 1-10 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part5 && numberQuePart + model.Number > 30)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 5 ranges from 1-30 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part6 && numberQuePart + model.Number > 4)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 6 ranges from 1-4 questions",
                    Success = false
                };
            }

            if (model.Part == (int)PartEnum.Part7 && numberQuePart + model.Number > 15)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Part 7 ranges from 1-15 questions",
                    Success = false
                };
            }

            var selectedQuestions = new List<long>();

            while (selectedQuestions.Count < model.Number && model.Level > 0)
            {
                int remaining = model.Number - selectedQuestions.Count;

                int totalQuestions = _unit.QuesToeic
                                          .Find(q => q.Part == model.Part && q.Level == model.Level && !existQueIds.Contains(q.QuesId))
                                          .Count();

                if (totalQuestions > 0)
                {
                    var random = new Random();
                    var randomQuestions = _unit.QuesToeic
                                               .Find(q => q.Part == model.Part && q.Level == model.Level && !existQueIds.Contains(q.QuesId))
                                               .Skip(random.Next(0, Math.Max(0, totalQuestions - remaining)))
                                               .Take(remaining)
                                               .Select(q => q.QuesId)
                                               .Where(quesId => !selectedQuestions.Contains(quesId))
                                               .ToList();

                    selectedQuestions.AddRange(randomQuestions);
                }

                model.Level--;
            }

            var newRandomQues = selectedQuestions.Select(qId => new RandomQuesToeicDto
            {
                HomeworkId = model.HomeworkId,
                QuesToeicId = qId
            }).ToList();

            foreach (var ran in newRandomQues)
            {
                var response = await CreateAsync(ran);
                if (!response.Success) return response;
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };

        }

        public async Task<Response> HandleCreateHwWithLevelAsync(long homeworkId, List<RandomPartWithLevelDto> models)
        {

            models.ForEach((item) =>
            {
                item.HomeworkId = homeworkId;
            });

            await _unit.BeginTransAsync();

            try
            {
                foreach (var model in models)
                {
                    var randomDto = new RandomPartDto()
                    {
                        HomeworkId = model.HomeworkId,
                        RoadMapExamId = model.RoadMapExamId,
                        Part = model.Part,
                    };

                    if (model.NumNormal != 0)
                    {
                        randomDto.Number = model.NumNormal;
                        randomDto.Level = (int)LevelEnum.Normal;
                        var createRes = await HandleCreateHwWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }

                    if (model.NumIntermediate != 0)
                    {
                        randomDto.Number = model.NumIntermediate;
                        randomDto.Level = (int)LevelEnum.Intermediate;
                        var createRes = await HandleCreateHwWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }

                    if (model.NumHard != 0)
                    {
                        randomDto.Number = model.NumHard;
                        randomDto.Level = (int)LevelEnum.Hard;
                        var createRes = await HandleCreateHwWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }

                    if (model.NumVeryHard != 0)
                    {
                        randomDto.Number = model.NumVeryHard;
                        randomDto.Level = (int)LevelEnum.VeryHard;
                        var createRes = await HandleCreateHwWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }
                }

                var homeworkModel = _unit.Homework.GetById(homeworkId);
                TimeOnly maxTime = new TimeOnly(2, 0, 0);

                TimeOnly finalTime;
                TimeOnly originalTime = homeworkModel.ExpectedTime;

                if (originalTime > maxTime)
                {
                    finalTime = maxTime;
                }
                else
                {
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
                }

                homeworkModel.Time = finalTime;
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

        public async Task<Response> HandleCreateRmWithLevelAsync(long examId, List<RandomPartWithLevelDto> models)
        {
            await _unit.BeginTransAsync();

            models.ForEach((item) =>
            {
                item.RoadMapExamId = examId;
            });

            try
            {
                foreach (var model in models)
                {
                    var randomDto = new RandomPartDto()
                    {
                        HomeworkId = model.HomeworkId,
                        RoadMapExamId = model.RoadMapExamId,
                        Part = model.Part,
                    };

                    if (model.NumNormal != 0)
                    {
                        randomDto.Number = model.NumNormal;
                        randomDto.Level = (int)LevelEnum.Normal;
                        var createRes = await HandleCreateRmWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }

                    if (model.NumIntermediate != 0)
                    {
                        randomDto.Number = model.NumIntermediate;
                        randomDto.Level = (int)LevelEnum.Intermediate;
                        var createRes = await HandleCreateRmWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }

                    if (model.NumHard != 0)
                    {
                        randomDto.Number = model.NumHard;
                        randomDto.Level = (int)LevelEnum.Hard;
                        var createRes = await HandleCreateRmWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }

                    if (model.NumVeryHard != 0)
                    {
                        randomDto.Number = model.NumVeryHard;
                        randomDto.Level = (int)LevelEnum.VeryHard;
                        var createRes = await HandleCreateRmWithLevelAsync(randomDto);
                        if (!createRes.Success) return createRes;
                    }
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

        public async Task<Response> HandleCreateRmByListAsync(List<RandomQuesToeicDto> models)
        {
            await _unit.BeginTransAsync();

            try
            {
                foreach (var model in models)
                {
                    var queModel = _unit.QuesToeic.GetById(model.QuesToeicId);

                    var numberQuePart = _unit.RandomQues
                                             .Include(r => r.QuesToeic)
                                             .Where(r => r.RoadMapExamId == model.RoadMapExamId && r.QuesToeic.Part == queModel.Part)
                                             .Count();

                    if (queModel.Part == (int)PartEnum.Part1 && numberQuePart + 1 > 6)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 1 ranges from 1-6 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part2 && numberQuePart + 1 > 25)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 2 ranges from 1-25 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part3 && numberQuePart + 1 > 13)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 3 ranges from 1-13 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part4 && numberQuePart + 1 > 10)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 4 ranges from 1-10 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part5 && numberQuePart + 1 > 30)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 5 ranges from 1-30 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part6 && numberQuePart + 1 > 4)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 6 ranges from 1-4 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part7 && numberQuePart + 1 > 15)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 7 ranges from 1-15 questions",
                            Success = false
                        };
                    }


                    var createRes = await CreateAsync(model, false);
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

        public async Task<Response> HandleCreateHwByListAsync(List<RandomQuesToeicDto> models)
        {
            await _unit.BeginTransAsync();

            try
            {
                foreach (var model in models)
                {
                    var queModel = _unit.QuesToeic.GetById(model.QuesToeicId);

                    var numberQuePart = _unit.RandomQues
                                             .Include(r => r.QuesToeic)
                                             .Where(r => r.HomeworkId == model.HomeworkId && r.QuesToeic.Part == queModel.Part)
                                             .Count();

                    if (queModel.Part == (int)PartEnum.Part1 && numberQuePart + 1 > 6)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 1 ranges from 1-6 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part2 && numberQuePart + 1 > 25)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 2 ranges from 1-25 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part3 && numberQuePart + 1 > 13)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 3 ranges from 1-13 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part4 && numberQuePart + 1 > 10)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 4 ranges from 1-10 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part5 && numberQuePart + 1 > 30)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 5 ranges from 1-30 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part6 && numberQuePart + 1 > 4)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 6 ranges from 1-4 questions",
                            Success = false
                        };
                    }

                    if (queModel.Part == (int)PartEnum.Part7 && numberQuePart + 1 > 15)
                    {

                        return new Response()
                        {
                            StatusCode = System.Net.HttpStatusCode.BadRequest,
                            Message = "Part 7 ranges from 1-15 questions",
                            Success = false
                        };
                    }


                    var createRes = await CreateAsync(model, false);
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

        public async Task<Response> GetByDefaultRmAsync(long examId)
        {
            var quesModels = await _unit.RandomQues
                                           .Include(r => r.QuesToeic)
                                           .Where(r => r.RoadMapExamId == examId)
                                           .Select(r => r.QuesToeic)
                                           .ToListAsync();

            foreach (var ran in quesModels)
            {
                await _unit.QuesToeic.LoadSubQuesWithAnswer(ran);
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<QuesToeicResDto>>(quesModels.OrderBy(q => q.Part)),
                Success = true
            };
        }

        public async Task<Response> GetNumberByHwAsync(long homeworkId)
        {
            var randomQues = await _unit.RandomQues
                                        .Include(r => r.QuesToeic)
                                        .Where(r => r.HomeworkId == homeworkId)
                                        .ToListAsync();

            var numQuesToeic = new NumQuesToeicResDto()
            {
                Part1 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part1).Count(),
                Part2 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part2).Count(),
                Part3 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part3).Count(),
                Part4 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part4).Count(),
                Part5 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part5).Count(),
                Part6 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part6).Count(),
                Part7 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part7).Count(),
            };

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = numQuesToeic,
                Success = true
            };
        }

        public async Task<Response> GetNumberByRmAsync(long roadMapExamId)
        {
            var randomQues = await _unit.RandomQues
                                        .Include(r => r.QuesToeic)
                                        .Where(r => r.RoadMapExamId == roadMapExamId)
                                        .ToListAsync();

            var numQuesToeic = new NumQuesToeicResDto()
            {
                Part1 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part1).Count(),
                Part2 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part2).Count(),
                Part3 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part3).Count(),
                Part4 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part4).Count(),
                Part5 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part5).Count(),
                Part6 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part6).Count(),
                Part7 = randomQues.Where(r => r.QuesToeic.Part == (int)PartEnum.Part7).Count(),
            };

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = numQuesToeic,
                Success = true
            };
        }
    }
}
