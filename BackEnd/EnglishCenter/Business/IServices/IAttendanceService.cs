using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAttendanceService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByClassAsync(string classId);
        public Task<Response> GetByLessonAsync(long lessonId);
        public Task<Response> ChangeIsAttendedAsync(long id, bool isAttended);
        public Task<Response> ChangeIsPermittedAsync(long id, bool isPermitted);
        public Task<Response> ChangeIsLateAsync(long id, bool isLate);
        public Task<Response> ChangeIsLeavedAsync(long id, bool isLeaved);
        public Task<Response> CreateAsync(AttendanceDto model);
        public Task<Response> HandleCreateByLessonAsync(long lessonId);
        public Task<Response> HandleAttendedAllAsync(long lessonId);
        public Task<Response> UpdateAsync(long id, AttendanceDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
