using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ILessonService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeClassAsync(long id, string classId);
        public Task<Response> ChangeDateAsync(long id, string dateOnlyStr);
        public Task<Response> ChangeStartPeriodAsync(long id, int startPeriod);
        public Task<Response> ChangeEndPeriodAsync(long id, int endPeriod);
        public Task<Response> ChangeTopicAsync(long id, string topic);
        public Task<Response> ChangeClassRoomAsync(long id, long classRoomId);
        public Task<Response> CreateAsync(LessonDto lessonModel);
        public Task<Response> UpdateAsync(long id, LessonDto lessonModel);
        public Task<Response> DeleteAsync(long id);
    }
}
