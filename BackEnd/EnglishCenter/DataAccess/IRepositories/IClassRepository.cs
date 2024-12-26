using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        public Task<List<Class>?> GetClassesWithTeacherAsync(string teacherId);
        public Task<bool> ChangeTeacherAsync(Class model, string teacherId);
        public Task<bool> ChangeCourseAsync(Class model, string courseId);
        public Task<bool> ChangeStartTimeAsync(Class model, DateOnly startTime);
        public Task<bool> ChangeEndTimeAsync(Class model, DateOnly endTime);
        public Task<bool> ChangeMaxNumAsync(Class model, int maxNum);
        public Task<bool> ChangeImageAsync(Class model, IFormFile image);
        public Task<bool> ChangeDescriptionAsync(Class model, string newDes);
        public Task<bool> ChangeStatusAsync(Class model, ClassEnum status);
    }
}
