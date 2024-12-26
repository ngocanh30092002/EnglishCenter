using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;

namespace EnglishCenter.Business.Services.Exams
{
    public class RoadMapExamService : IRoadMapExamService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unit;
        private readonly IRandomQueToeicService _randomService;

        public RoadMapExamService(IMapper mapper, IUnitOfWork unit, IRandomQueToeicService randomService)
        {
            _mapper = mapper;
            _unit = unit;
            _randomService = randomService;
        }

        public async Task<Response> ChangeNameAsync(long id, string newName)
        {
            var examModel = _unit.RoadMapExams.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic exams",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.RoadMapExams.ChangeNameAsync(examModel, newName);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change name failed",
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

        public async Task<Response> ChangeRoadMapAsync(long id, long roadMapId)
        {
            var examModel = _unit.RoadMapExams.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic exams",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.RoadMapExams.ChangeRoadMapAsync(examModel, roadMapId);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change road map failed",
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

        public async Task<Response> ChangeTimeAsync(long id, double timeMinute)
        {
            var examModel = _unit.RoadMapExams.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic exams",
                    Success = false
                };
            }

            var isChangeSuccess = await _unit.RoadMapExams.ChangeTimeAsync(examModel, timeMinute);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Change time minutes failed",
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

        public async Task<Response> CreateAsync(RoadMapExamDto model)
        {
            var roadMapModel = _unit.RoadMaps.GetById(model.RoadMapId);
            if (roadMapModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any road maps",
                    Success = false
                };
            }

            if (model.Point < 0 || model.Point > 990)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Point is invalid",
                    Success = false
                };
            }

            if (model.TimeMinutes < 0)
            {

                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Time is invalid",
                    Success = false
                };
            }

            var examModel = _mapper.Map<RoadMapExam>(model);
            examModel.DirectionId = 1;
            examModel.CompletedNum = 0;

            _unit.RoadMapExams.Add(examModel);
            await _unit.CompleteAsync();


            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = examModel.RoadMapExamId,
                Success = true
            };
        }

        public async Task<Response> DeleteAsync(long id)
        {
            var examModel = _unit.RoadMapExams.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic exams",
                    Success = false
                };
            }

            var randomQueIds = _unit.RandomQues
                                    .Find(q => q.RoadMapExamId == id)
                                    .Select(q => q.Id)
                                    .ToList();

            foreach (var randomQueId in randomQueIds)
            {
                var deleteRes = await _randomService.DeleteAsync(randomQueId);
                if (!deleteRes.Success) return deleteRes;
            }

            _unit.RoadMapExams.Remove(examModel);
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
            var examModels = _unit.RoadMapExams.GetAll();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<RoadMapExamResDto>>(examModels),
                Success = true
            });
        }

        public Task<Response> GetAsync(long id)
        {
            var examModel = _unit.RoadMapExams.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<RoadMapExamResDto>(examModel),
                Success = true
            });
        }

        public Task<Response> GetByRoadMapAsync(long roadMapId)
        {
            var examModels = _unit.RoadMapExams.Find(r => r.RoadMapId == roadMapId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<RoadMapExamResDto>>(examModels),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, RoadMapExamDto model)
        {
            var examModel = _unit.RoadMapExams.GetById(id);
            if (examModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any toeic exams",
                    Success = false
                };
            }

            await _unit.BeginTransAsync();
            try
            {
                if (examModel.RoadMapId != model.RoadMapId)
                {
                    var response = await ChangeRoadMapAsync(id, model.RoadMapId);
                    if (!response.Success) return response;
                }

                if (examModel.Name != model.Name)
                {
                    var response = await ChangeNameAsync(id, model.Name);
                    if (!response.Success) return response;
                }

                if (examModel.TimeMinutes != model.TimeMinutes)
                {
                    var response = await ChangeTimeAsync(id, model.TimeMinutes);
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
    }
}
