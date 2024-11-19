using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IToeicDirectionService
    {
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeIntroduceListeningAsync(long id, string introduce);
        public Task<Response> ChangeIntroduceReadingAsync(long id, string introduce);
        public Task<Response> ChangeImageAsync(long id, IFormFile imageFile);
        public Task<Response> ChangePartAsync(long id, string value, int part);
        public Task<Response> ChangeAudioAsync(long id, IFormFile audioFile, int part);
        public Task<Response> CreateAsync(ToeicDirectionDto model);
        public Task<Response> DeleteAsync(long id);
        public Task<Response> UpdateAsync(long id, ToeicDirectionDto model);
    }
}
