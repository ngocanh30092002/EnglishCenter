using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IClassMaterialService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByClassAsync(string classId);
        public Task<Response> ChangeTitleAsync(long id, string newTitle);
        public Task<Response> ChangeFilePathAsync(long id, IFormFile file);
        public Task<Response> ChangeUploadByAsync(long id, string uploadBy);
        public Task<Response> CreateAsync(string userId, ClassMaterialDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
