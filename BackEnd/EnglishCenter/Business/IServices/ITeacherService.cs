using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface ITeacherService
    {
        public Task<Response> GetFullNameAsync(string userId);
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(string userId);
        public Task<Response> ChangeBackgroundImageAsync(IFormFile file, string userId);
        public Task<Response> ChangeTeacherImageAsync(IFormFile file, string userId);
    }
}
