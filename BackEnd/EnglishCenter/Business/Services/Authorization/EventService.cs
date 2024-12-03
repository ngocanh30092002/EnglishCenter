using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.Services.Authorization
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public EventService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<Response> CreateAsync(string userId, EventDto model)
        {
            if (!_unit.Students.IsExist(x => x.UserId == userId))
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            var scheduleEvent = _mapper.Map<ScheduleEvent>(model);
            scheduleEvent.UserId = userId;

            _unit.Events.Add(scheduleEvent);
            await _unit.CompleteAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> DeleteAsync(long eventId)
        {
            var eventModel = _unit.Events.GetById(eventId);

            if (eventModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            _unit.Events.Remove(eventModel);
            await _unit.CompleteAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public Task<Response> GetEventAsync(long eventId)
        {
            var eventModel = _unit.Events.GetById(eventId);

            return Task.FromResult(new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<EventDto>(eventModel)
            });
        }

        public async Task<Response> GetEventsAsync(string userId)
        {
            if (!_unit.Students.IsExist(x => x.UserId == userId))
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            var events = await _unit.Events.GetEventsByUserAsync(userId);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EventDto>>(events)
            };
        }

        public async Task<Response> GetEventsInRangeAsync(DateOnly startTime, DateOnly endTime)
        {
            var events = await _unit.Events.GetEventsInRangeAsync(startTime, endTime);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = events.ToList()
            };
        }

        public async Task<Response> GetEventsWithDateAsync(string userId, DateOnly date)
        {
            bool isValidStudent = _unit.Students.IsExist(x => x.UserId == userId);
            if (!isValidStudent)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any users"
                };
            }

            var events = await _unit.Events.GetEventsWithDateAsync(userId, date);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = _mapper.Map<List<EventDto>>(events)
            };
        }

        public async Task<Response> UpdateAsync(long eventId, EventDto model)
        {
            var eventModel = _unit.Events.GetById(eventId);

            if (eventModel == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            var scheduleEventModel = _mapper.Map<ScheduleEvent>(model);

            var isSuccess = await _unit.Events.UpdateAsync(eventId, scheduleEventModel);

            if (!isSuccess)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            await _unit.CompleteAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }
    }
}
