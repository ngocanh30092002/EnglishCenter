using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IAttemptRecordService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetResultAsync(long attemptId, string userId);
        public Task<Response> GetResultScoreAsync(long attemptId, string userId);
        public Task<Response> GetByRoadMapQuestionAsync(long attemptId, string userId);
        public Task<Response> ChangeSelectedAnswerAsync(long id, string? selectedAnswer);
        public Task<Response> CreateAsync(AttemptRecordDto model);
        public Task<Response> UpdateAsync(long id, AttemptRecordDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
