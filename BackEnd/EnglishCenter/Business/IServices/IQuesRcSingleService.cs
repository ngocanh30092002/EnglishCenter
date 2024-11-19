using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesRcSingleService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeQuantityAsync(long quesId, int quantity);
        public Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly);
        public Task<Response> ChangeImageAsync(long quesId, IFormFile imageFile);
        public Task<Response> CreateAsync(QuesRcSingleDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesRcSingleDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
