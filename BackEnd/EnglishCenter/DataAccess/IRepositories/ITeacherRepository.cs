using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        public Task<string> GetFullNameAsync(string userId);
        public string GetFullName(Teacher teacher);
    }
}
