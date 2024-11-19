using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesRcSenMediaService 
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeAudioAsync(long quesId, IFormFile? audioFile);
        public Task<Response> ChangeImageAsync(long quesId, IFormFile imageFile);
        public Task<Response> ChangeAnswerAAsync(long quesId, string newAnswer);
        public Task<Response> ChangeAnswerBAsync(long quesId, string newAnswer);
        public Task<Response> ChangeAnswerCAsync(long quesId, string newAnswer);
        public Task<Response> ChangeAnswerDAsync(long quesId, string newAnswer);
        public Task<Response> ChangeQuestionAsync(long quesId, string newQuestion);
        public Task<Response> ChangeAnswerAsync(long quesId, long answerId);
        public Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly);
        public Task<Response> CreateAsync(QuesRcSenMediaDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesRcSenMediaDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
