using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        public Task<List<Class>?> GetClassesWithTeacherAsync(string teacherId);
        public Task<Response> UpdateAsync(string classId, ClassDto model);
        public Task<bool> ChangeCourseAsync(Class model, string courseId);
        public Task<bool> ChangeStartTimeAsync(Class model, DateOnly startTime);
        public Task<bool> ChangeEndTimeAsync(Class model, DateOnly endTime);
        public Task<bool> ChangeMaxNumAsync(Class model, int maxNum);
        public Task<bool> ChangeImageAsync(Class model, IFormFile image);

    }
}
