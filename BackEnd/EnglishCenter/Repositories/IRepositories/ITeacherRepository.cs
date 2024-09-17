using EnglishCenter.Models;

namespace EnglishCenter.Repositories.IRepositories
{
    public interface ITeacherRepository
    {
        public Task<Response> GetFullNameAsync(string userId);
    }
}
