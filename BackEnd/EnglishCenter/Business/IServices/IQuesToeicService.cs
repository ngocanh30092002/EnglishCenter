using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesToeicService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByToeicAsync(long toeicId);
        public Task<Response> GetTotalNumberSentences(long toeicId);
        public Task<Response> ChangeNoNumAsync(long id, int noNum);
        public Task<Response> ChangeLevelAsync(long id, int level);
        public Task<Response> ChangeAudioAsync(long id, IFormFile? audioFile);
        public Task<Response> ChangeImage1Async(long id, IFormFile? imageFile);
        public Task<Response> ChangeImage2Async(long id, IFormFile? imageFile);
        public Task<Response> ChangeImage3Async(long id, IFormFile? imageFile);
        public Task<Response> ChangeGroupAsync(long id, bool isGroup);
        public Task<int> NextNoNumAsync(long toeicId, int part);
        public Task<Response> CreateAsync(QuesToeicDto model);
        public Task<Response> UpdateAsync(long id, QuesToeicDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
