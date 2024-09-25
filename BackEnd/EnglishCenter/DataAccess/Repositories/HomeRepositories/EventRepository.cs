using AutoMapper;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.HomeRepositories
{
    public class EventRepository : GenericRepository<ScheduleEvent>, IEventRepository
    {

        public EventRepository(EnglishCenterContext context) : base(context)
        {

        }

        public async Task<List<ScheduleEvent>?> GetEventsByUserAsync(string userId)
        {
            var student = await context.Students
                                        .Include(s => s.ScheduleEvents)
                                        .FirstOrDefaultAsync(s => s.UserId == userId);

            return student?.ScheduleEvents?.ToList();
        }

        public async Task<List<ScheduleEvent>?> GetEventsWithDateAsync(string userId, DateOnly date)
        {
            var scheduleEvents = await context.ScheduleEvents
                                        .Where(e => e.UserId == userId && e.Date == date)
                                        .ToListAsync();

            return scheduleEvents;
        }

        public async Task<List<bool>> GetEventsInRangeAsync(DateOnly startTime, DateOnly endTime)
        {
            var scheduleEvents = await context.ScheduleEvents
                                        .Where(e => e.Date >= startTime && e.Date <= endTime)
                                        .ToListAsync();

            int betweenDay = (endTime.ToDateTime(TimeOnly.MinValue) - startTime.ToDateTime(TimeOnly.MinValue)).Days;

            bool[] returnArr = new bool[betweenDay + 1];
            bool isExist = false;

            for (int i = 0; i <= betweenDay; i++)
            {
                isExist = scheduleEvents.Any(e => e.Date == startTime.AddDays(i));
                returnArr[i] = isExist;
            }

            return returnArr.ToList();
        }

        public async Task<bool> UpdateAsync(long eventId, ScheduleEvent model)
        {
            var scheduleEvent = await context.ScheduleEvents.FindAsync(eventId);

            if (scheduleEvent == null) return false;

            scheduleEvent.Title = model.Title;
            scheduleEvent.Date = model.Date;
            scheduleEvent.StartTime = model.StartTime;
            scheduleEvent.EndTime = model.EndTime;

            context.ScheduleEvents.Update(scheduleEvent);

            return true;
        }
    }
}
