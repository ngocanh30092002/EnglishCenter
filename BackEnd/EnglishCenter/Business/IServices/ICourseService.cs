using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ICourseService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(string courseId);
        public Task<Response> GetPreviousAsync(string courseId);
        public Task<Response> CreateAsync(CourseDto model);
        public Task<Response> UpdateAsync(string courseId, CourseDto model);
        public Task<Response> UploadImageAsync(string courseId, IFormFile file);
        public Task<Response> UploadImageThumbnailAsync(string courseId, IFormFile file);
        public Task<Response> DeleteAsync(string courseId);
        public Task<Response> ChangePriorityAsync(string courseId, int priority);
        public Task<Response> CheckIsQualifiedAsync(string userId, string courseId);
    }
}
