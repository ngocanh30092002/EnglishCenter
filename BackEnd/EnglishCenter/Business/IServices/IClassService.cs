using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IClassService
    {
        public Task<Response> CreateAsync(ClassDto model);
        public Task<Response> UpdateAsync(string classId, ClassDto model);
        public Task<Response> DeleteAsync(string classId);
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(string classId);
        public Task<Response> GetClassesWithTeacherAsync(string userId);
        public Task<Response> ChangeCourseAsync(string classId, string courseId);
        public Task<Response> ChangeStartTimeAsync(string classId, DateOnly startTime);
        public Task<Response> ChangeEndTimeAsync(string classId, DateOnly endTime);
        public Task<Response> ChangeMaxNumAsync(string classId, int maxNum);
        public Task<Response> ChangeImageAsync(string classId, IFormFile image);
        public Task<bool> IsClassOfTeacherAsync(string userId, string classId);
    }
}
