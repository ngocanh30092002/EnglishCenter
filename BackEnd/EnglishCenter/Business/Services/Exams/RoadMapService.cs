using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using EnglishCenter.Presentation.Models.ResDTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Exams
{
    public class RoadMapService : IRoadMapService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public RoadMapService(IMapper mapper, IUnitOfWork unit)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<Response> CreateAsync(RoadMapDto model)
        {
            var courseModel = _unit.Courses.GetById(model.CourseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "",
                    Success = false
                };
            }

            var maxRoadMaps = _unit.RoadMaps
                                   .Find(r => r.CourseId == courseModel.CourseId)
                                   .OrderByDescending(r => r.Order)
                                   .FirstOrDefault();


            var roadMapModel = _mapper.Map<RoadMap>(model);

            if (model.Order.HasValue)
            {
                int maxOrder = maxRoadMaps == null ? 1 : maxRoadMaps.Order;
                if (model.Order.Value <= 0 || model.Order.Value > maxOrder + 1)
                {
                    return new Response()
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "Order is invalid",
                        Success = false
                    };
                }
                var greaterRoadMaps = _unit.RoadMaps
                                           .Find(r => r.CourseId == courseModel.CourseId && r.Order >= model.Order.Value)
                                           .OrderBy(r => r.Order);

                foreach (var item in greaterRoadMaps)
                {
                    item.Order = item.Order + 1;
                }

                roadMapModel.Order = model.Order.Value;
            }
            else
            {
                roadMapModel.Order = maxRoadMaps == null ? 1 : maxRoadMaps.Order + 1;
            }

            _unit.RoadMaps.Add(roadMapModel);

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
            var roadMapModel = _unit.RoadMaps.GetById(id);
            if (roadMapModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any road maps",
                    Success = false
                };
            }

            var otherRoadMap = _unit.RoadMaps.Find(r => r.RoadMapId != id);

            int i = 1;
            foreach (var item in otherRoadMap)
            {
                item.Order = i++;
            }

            _unit.RoadMaps.Remove(roadMapModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> ChangeOrderAsync(long id, int order)
        {
            var roadModel = _unit.RoadMaps.GetById(id);

            if (roadModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any road maps",
                    Success = false
                };
            }

            int maxOrder = _unit.RoadMaps.GetAll().Max(r => r.Order);
            if (order < 1 || order > maxOrder + 1)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Order is invalid",
                    Success = false
                };
            }

            int oldOrder = roadModel.Order;

            var affectedItems = _unit.RoadMaps.Find(r => r.RoadMapId != id && r.CourseId == roadModel.CourseId).ToList();

            foreach (var item in affectedItems)
            {
                if (order < oldOrder && item.Order >= order && item.Order < oldOrder)
                {
                    item.Order++;
                }
                else if (order > oldOrder && item.Order > oldOrder && item.Order <= order)
                {
                    item.Order--;
                }
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public async Task<Response> GetAllAsync()
        {
            var roadMaps = await _unit.RoadMaps
                                      .Include(r => r.RoadMapExams)
                                      .ToListAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<RoadMapResDto>>(roadMaps.OrderByDescending(r => r.Order)),
                Success = true
            };
        }

        public Task<Response> GetAsync(long id)
        {
            var roadMap = _unit.RoadMaps.GetById(id);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<RoadMapDto>(roadMap),
                Success = true
            });
        }

        public Task<Response> GetByCourseAsync(string courseId)
        {
            var roadMaps = _unit.RoadMaps.Find(r => r.CourseId == courseId);

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<RoadMapDto>>(roadMaps.OrderBy(r => r.Order)),
                Success = true
            });
        }

        public async Task<Response> UpdateAsync(long id, RoadMapDto model)
        {
            var roadMapModel = _unit.RoadMaps.GetById(id);
            if (roadMapModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any road maps",
                    Success = false
                };
            }

            var courseModel = _unit.Courses.GetById(model.CourseId);
            if (courseModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any courses",
                    Success = false
                };
            }

            if (model.Order.HasValue)
            {
                var changeRes = await ChangeOrderAsync(id, model.Order.Value);
                if (!changeRes.Success) return changeRes;
            }

            roadMapModel.CourseId = model.CourseId;
            roadMapModel.Name = model.Name;

            await _unit.CompleteAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
            };
        }

        public Task<Response> GetCourseIdsAsync()
        {
            var courseIds = _unit.RoadMaps.GetAll().Select(t => t.CourseId).Distinct().ToList();

            return Task.FromResult(new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = courseIds,
                Success = true
            });
        }
    }
}
