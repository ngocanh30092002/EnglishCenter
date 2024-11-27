using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        public Task<bool> ChangeTeacherImageAsync(IFormFile file, Teacher teacher);
        public Task<bool> ChangeBackgroundImageAsync(IFormFile file, Teacher teacher);
        public Task<string> GetFullNameAsync(string userId);
        public string GetFullName(Teacher teacher);
    }
}
