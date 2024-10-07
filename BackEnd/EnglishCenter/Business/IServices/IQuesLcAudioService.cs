using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesLcAudioService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeAnswerAsync(long quesId, long answerId);
        public Task<Response> ChangeAudioAsync(long quesId, IFormFile audioFile);
        public Task<Response> CreateAsync(QuesLcAudioDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesLcAudioDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
