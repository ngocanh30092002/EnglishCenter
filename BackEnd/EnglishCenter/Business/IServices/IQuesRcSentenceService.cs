using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IQuesRcSentenceService
    {
        public Task<Response> GetAsync(long quesId);
        public Task<Response> GetOtherQuestionByAssignmentAsync(long assignmentId);
        public Task<Response> GetOtherQuestionByHomeworkAsync(long homeworkId);
        public Task<Response> GetAllAsync();
        public Task<Response> ChangeAnswerAAsync(long quesId, string newAnswer);
        public Task<Response> ChangeAnswerBAsync(long quesId, string newAnswer);
        public Task<Response> ChangeAnswerCAsync(long quesId, string newAnswer);
        public Task<Response> ChangeAnswerDAsync(long quesId, string newAnswer);
        public Task<Response> ChangeQuestionAsync(long quesId, string newQuestion);
        public Task<Response> ChangeAnswerAsync(long quesId, long answerId);
        public Task<Response> ChangeTimeAsync(long quesId, TimeOnly timeOnly);
        public Task<Response> ChangeLevelAsync(long quesId, int level);
        public Task<Response> CreateAsync(QuesRcSentenceDto queModel);
        public Task<Response> UpdateAsync(long quesId, QuesRcSentenceDto queModel);
        public Task<Response> DeleteAsync(long quesId);
    }
}
