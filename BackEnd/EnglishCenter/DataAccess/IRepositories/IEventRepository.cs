using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IEventRepository : IGenericRepository<ScheduleEvent>
    {
        public Task<List<ScheduleEvent>?> GetEventsByUserAsync(string userId);
        public Task<List<ScheduleEvent>?> GetEventsWithDateAsync(string userId, DateOnly date);
        public Task<bool> UpdateAsync(long eventId, ScheduleEvent model);
        public Task<List<bool>> GetEventsInRangeAsync(DateOnly startTime, DateOnly endTime);
    }
}
