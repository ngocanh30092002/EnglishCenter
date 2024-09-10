using EnglishCenter.Models;
using EnglishCenter.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface ICourseRepository
    {
        public Task<Response> CreateCourseAsync(CourseDtoModel model);
        public Task<Response> UpdateCourseAsync(string courseId, CourseDtoModel model);
        public Task<Response> DeleteCourseAsync(string courseId);
        public Task<Response> ChangePriorityAsync(string courseId, int priority);
        public Task<Response> GetCoursesAsync();
        public Task<Response> GetCourseAsync(string courseId);
        public Task<Response> UploadCourseImageAsync(string courseId, IFormFile image);
    }
}
