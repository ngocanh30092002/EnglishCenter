using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IClassScheduleService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeStartPeriodAsync(long id, int start);
        public Task<Response> ChangeEndPeriodAsync(long id, int end);
        public Task<Response> ChangeDayOfWeekAsync(long id, int dayOfWeek);
        public Task<Response> ChangeClassAsync(long id, string classId);
        public Task<Response> ChangeClassRoomAsync(long id, long classRoomId);
        public Task<Response> HandleCreateLessonAsync(string classId);
        public Task<Response> CreateAsync(ClassScheduleDto classSchedule);
        public Task<Response> UpdateAsync(long id, ClassScheduleDto classSchedule);
        public Task<Response> DeleteAsync(long id);
    }
}
