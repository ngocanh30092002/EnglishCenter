using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAnswerLcAudioService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeQuestionAsync(long id, string newQues);
        public Task<Response> ChangeAnswerAAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerBAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerCAsync(long id, string newAnswer);
        public Task<Response> ChangeCorrectAnswerAsync(long id, string newAnswer);
        public Task<Response> CreateAsync(AnswerLcAudioDto model);
        public Task<Response> UpdateAsync(long id, AnswerLcAudioDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
