using EnglishCenter.Presentation.Models;

namespace EnglishCenter.Business.IServices
{
    public interface IChatFileService
    {
        public Task<Response> CreateAsync(string senderId, IFormFile file);
        public Task<Response> DeleteAsync(long id);
    }
}
