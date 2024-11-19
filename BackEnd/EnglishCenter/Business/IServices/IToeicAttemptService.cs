using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IToeicAttemptService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByUserAsync(string userId);
        public Task<Response> ChangeUserAsync(long id, string userId);
        public Task<Response> ChangeToeicAsync(long id, long toeicId);
        public Task<Response> ChangeListeningScoreAsync(long id, int score);
        public Task<Response> ChangeReadingScoreAsync(long id, int score);
        public Task<Response> CreateAsync(ToeicAttemptDto model);
        public Task<Response> UpdateAsync(long id, ToeicAttemptDto model);
        public Task<Response> DeleteAsync(long id);
        public Task<Response> HandleSubmitToeicAsync(long attemptId, ToeicAttemptDto model);
    }
}
