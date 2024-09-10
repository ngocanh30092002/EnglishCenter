using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IEventRepository
    {
        public Task<Response> CreateScheduleEventAsync(string userId, EventDtoModel model);
        public Task<Response> GetScheduleEventsAsync(string userId);
        public Task<Response> GetScheduleEventsWithDateAsync(string userId, DateOnly date);
        public Task<Response> GetScheduleEventAsync(long eventId);
        public Task<Response> RemoveScheduleEventAsync(long eventId);
        public Task<Response> UpdateScheduleEventAsync(EventDtoModel model);
        public Task<Response> GetNotificationEventsInRange(DateOnly startTime, DateOnly endTime);
    }
}
