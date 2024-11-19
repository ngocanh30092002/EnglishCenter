using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAnswerRcSentenceService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeAnswerAAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerBAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerCAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerDAsync(long id, string newAnswer);
        public Task<Response> ChangeExplanationAsync(long id, string newExplanation);
        public Task<Response> ChangeQuestionAsync(long id, string newQuestion);
        public Task<Response> ChangeCorrectAnswerAsync(long id, string newCorrectAnswer);
        public Task<Response> CreateAsync(AnswerRcSentenceDto model);
        public Task<Response> UpdateAsync(long id, AnswerRcSentenceDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
