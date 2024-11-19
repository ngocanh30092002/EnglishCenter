using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ISubToeicService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeAnswerAAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerBAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerCAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerDAsync(long id, string newAnswer);
        public Task<Response> ChangeAnswerAsync(long id, long answerId);
        public Task<Response> ChangeQuesNoAsync(long id, int queNo);
        public Task<Response> ChangeQuestionAsync(long id, string question);
        public Task<Response> CreateAsync(SubToeicDto model);
        public Task<Response> UpdateAsync(long id, SubToeicDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
