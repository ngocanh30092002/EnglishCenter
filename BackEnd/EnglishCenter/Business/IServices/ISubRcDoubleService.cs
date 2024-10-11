using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ISubRcDoubleService
    {
        public Task<Response> GetAsync(long subId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeAnswerAAsync(long subId, string newAnswer);
        public Task<Response> ChangeAnswerBAsync(long subId, string newAnswer);
        public Task<Response> ChangeAnswerCAsync(long subId, string newAnswer);
        public Task<Response> ChangeAnswerDAsync(long subId, string newAnswer);
        public Task<Response> ChangeQuestionAsync(long subId, string newQuestion);
        public Task<Response> ChangePreQuesAsync(long subId, long preId);
        public Task<Response> ChangeAnswerAsync(long subId, long answerId);
        public Task<Response> ChangeNoNumAsync(long subId, int noNum);
        public Task<Response> CreateAsync(SubRcDoubleDto subModel);
        public Task<Response> UpdateAsync(long subId, SubRcDoubleDto subModel);
        public Task<Response> DeleteAsync(long subId);
    }
}
