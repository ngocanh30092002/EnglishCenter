using System.Net.WebSockets;
using AutoMapper;
using EnglishCenter.Database;
using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using EnglishCenter.Models.IdentityModel;
using EnglishCenter.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Repositories.HomeRepositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EnglishCenterContext _context;
        private readonly IMapper _mapper;

        public EventRepository(EnglishCenterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> CreateScheduleEventAsync(string userId, EventDto model)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "User not found"
                };
            }

            var student = await _context.Students
                                        .Include(s => s.ScheduleEvents)
                                        .FirstOrDefaultAsync(s => s.UserId == userId);
        
            if(student == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            var scheduleEvent = _mapper.Map<ScheduleEvent>(model);
            student.ScheduleEvents.Add(scheduleEvent);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = ""
            };
        }

        public async Task<Response> GetNotificationEventsInRange(DateOnly startTime, DateOnly endTime)
        {
            var scheduleEvents = await _context.ScheduleEvents
                                        .Where(e => e.Date >= startTime && e.Date <= endTime)
                                        .ToListAsync();
            int betweenDay = (endTime.ToDateTime(TimeOnly.MinValue) - startTime.ToDateTime(TimeOnly.MinValue)).Days;

            bool[] returnArr = new bool[betweenDay + 1];
            bool isExist = false;

            for(int i = 0; i <= betweenDay; i++)
            {
                isExist = scheduleEvents.Any(e => e.Date == startTime.AddDays(i));
                returnArr[i] = isExist;
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = returnArr,
                Success = true
            };
        }

        public async Task<Response> GetScheduleEventAsync(long eventId)
        {
            var scheduleEvent = await _context.ScheduleEvents
                                        .FirstOrDefaultAsync(e => e.ScheduleId == eventId);

            return new Response()
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = scheduleEvent
            };
        }

        public async Task<Response> GetScheduleEventsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "User not found"
                };
            }

            var student = await _context.Students
                                        .Include(s => s.ScheduleEvents)
                                        .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null)
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any students"
                };
            }

            var eventsDtoModel = _mapper.Map<List<EventDto>>(student.ScheduleEvents);

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = eventsDtoModel
            };
        }

        public async Task<Response> GetScheduleEventsWithDateAsync(string userId, DateOnly date)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new Response()
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "User not found"
                };
            }

            var scheduleEvents = await _context.ScheduleEvents
                                        .Where(e => e.UserId == userId && e.Date == date)
                                        .ToListAsync();

            return new Response()
            {
                Success = true,
                Message = _mapper.Map<List<EventDto>>(scheduleEvents),
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }

        public async Task<Response> RemoveScheduleEventAsync(long eventId)
        {
            var scheduleEvent = await _context.ScheduleEvents.FindAsync(eventId);

            if (scheduleEvent == null)
            {
                return new Response()
                {
                    Message = "Can't find any schedule events",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            _context.ScheduleEvents.Remove(scheduleEvent);
            await _context.SaveChangesAsync();

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = ""
            };
        }

        public async Task<Response> UpdateScheduleEventAsync(EventDto model)
        {
            if (!model.ScheduleId.HasValue)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "EventId is required",
                    Success = false
                };
            }

            var scheduleEvent = await _context.ScheduleEvents.FindAsync(model.ScheduleId);

            if(scheduleEvent == null)
            {
                return new Response()
                {
                    Message = "Can't find any schedule events",
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
            }

            var scheduleModel = _mapper.Map<ScheduleEvent>(model);

            scheduleEvent.Title = scheduleModel.Title;
            scheduleEvent.Date = scheduleModel.Date;
            scheduleEvent.StartTime = scheduleModel.StartTime;
            scheduleEvent.EndTime = scheduleModel.EndTime;

            _context.ScheduleEvents.Update(scheduleEvent);

            await _context.SaveChangesAsync();

            return new Response()
            {
                Success = true,
                Message = "",
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
