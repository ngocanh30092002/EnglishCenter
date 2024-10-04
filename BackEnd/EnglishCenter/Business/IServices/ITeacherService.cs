using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface ITeacherService
    {
        public Task<Response> GetFullNameAsync(string userId);
        public Task<Response> GetAsync(string userId);
    }
}
