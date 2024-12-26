using AutoMapper;
using EnglishCenter.Business.IServices;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Global;
using EnglishCenter.Presentation.Hub;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Business.Services.Authorization
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unit;

        private readonly IMapper _mapper;
        private readonly EnglishCenterContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventService(IUnitOfWork unit, IMapper mapper, EnglishCenterContext context, IHubContext<NotificationHub> hubContext)
        {
            _unit = unit;
            _mapper = mapper;
            _context = context;
            _hubContext = hubContext;
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

        public async Task<Response> SendTaskInfoToNotiAsync(string userId, long scheduleId)
        {
            var scheduleModel = _unit.Events.GetById(scheduleId);
            if (scheduleModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find schedule events",
                    Success = false
                };
            }

            var group = _context.Groups
                                .Include(g => g.Students)
                                .FirstOrDefault(g => g.Name == GlobalVariable.SYSTEM);

            if (group == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Message = "Can't find any group",
                    Success = false
                };
            }


            if (scheduleModel.IsSend == false)
            {
                var notiModel = new Notification()
                {
                    Title = "Automatic notification",
                    Description = $"It's time to '{scheduleModel.Title}'. Let's get started.",
                    Time = DateTime.Now,
                    Image = "/notifications/images/automatic.svg",
                    LinkUrl = null
                };

                scheduleModel.IsSend = true;
                await _unit.CompleteAsync();

                _context.Notifications.Add(notiModel);
                await _context.SaveChangesAsync();

                _context.NotiStudents.Add(new NotiStudent()
                {
                    IsRead = false,
                    NotiId = notiModel.NotiId,
                    UserId = userId
                });

                await _context.SaveChangesAsync();
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "",
                Success = true
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
