using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        public Task<Response> UpdateAsync(string courseId, CourseDto model);
        public Task<bool> ChangePriorityAsync(Course courseModel, int priority);
        public Task<bool> UploadImageAsync(Course courseModel, IFormFile image);
        public Task<Course?> GetPreviousAsync(Course course);
    }
}
