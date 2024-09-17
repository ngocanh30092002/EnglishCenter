using EnglishCenter.Models;
using EnglishCenter.Models.DTO;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface IClassRepository
    {
        public Task<Response> CreateClassAsync(ClassDto model);
        public Task<Response> UpdateClassAsync(string classId, ClassDto model);
        public Task<Response> DeleteClassAsync(string classId);
        public Task<Response> GetClassesAsync();
        public Task<Response> GetClassAsync(string classId);
        public Task<Response> GetClassesWithTeacherAsync(string teacherId);
        public Task<Response> ChangeCourseAsync(string classId, string courseId);
        public Task<Response> ChangeStartTimeAsync(string classId, DateOnly startTime);
        public Task<Response> ChangeEndTimeAsync(string classId, DateOnly endTime);
        public Task<Response> ChangeMaxNumAsync(string classId, int maxNum);
        public Task<Response> ChangeImageAsync(string classId, IFormFile image);

    }
}
