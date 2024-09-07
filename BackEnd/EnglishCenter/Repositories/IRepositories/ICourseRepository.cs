using EnglishCenter.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface ICourseRepository
    {
        public Task<Response> CreateCourseAsync(Course model);
        public Task<Response> UpdateCourseAsync(string courseId, Course model);
        public Task<Response> DeleteCourseAsync(string courseId);
        public Task<Response> ChangePriorityAsync(string courseId, int priority);
        public Task<List<Course>> GetCoursesAsync();
        public Task<Course> GetCourseAsync(string courseId);
    }
}
