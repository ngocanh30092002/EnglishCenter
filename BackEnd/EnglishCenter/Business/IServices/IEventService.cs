using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IEventService
    {
        public Task<Response> CreateAsync(string userId, EventDto model);
        public Task<Response> UpdateAsync(long eventId, EventDto model);
        public Task<Response> DeleteAsync(long eventId);
        public Task<Response> GetEventsAsync(string userId);
        public Task<Response> GetEventsWithDateAsync(string userId, DateOnly date);
        public Task<Response> GetEventAsync(long eventId);
        public Task<Response> GetEventsInRangeAsync(DateOnly startTime, DateOnly endTime);
        public Task<Response> SendTaskInfoToNotiAsync(string userId, long scheduleId);
    }
}
