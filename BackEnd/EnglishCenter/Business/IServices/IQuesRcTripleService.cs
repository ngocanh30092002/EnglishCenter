using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesRcTripleService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeQuantityAsync(long quesId, int quantity);
        public Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly);
        public Task<Response> ChangeImage1Async(long quesId, IFormFile imageFile);
        public Task<Response> ChangeImage2Async(long quesId, IFormFile imageFile);
        public Task<Response> ChangeImage3Async(long quesId, IFormFile imageFile);
        public Task<Response> CreateAsync(QuesRcTripleDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesRcTripleDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
