using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IUserAttemptService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByUserAsync(string userId);
        public Task<Response> GetByToeicAsync(string userId);
        public Task<Response> GetByCourseAsync(string userId,string courseId);
        public Task<Response> ChangeUserAsync(long id, string userId);
        public Task<Response> ChangeToeicAsync(long id, long toeicId);
        public Task<Response> ChangeRoadMapExamAsync(long id, long roadMapExamId);
        public Task<Response> ChangeListeningScoreAsync(long id, int score);
        public Task<Response> ChangeReadingScoreAsync(long id, int score);
        public Task<Response> CreateAsync(UserAttemptDto model);
        public Task<Response> UpdateAsync(long id, UserAttemptDto model);
        public Task<Response> DeleteAsync(long id);
        public Task<Response> HandleSubmitToeicAsync(long attemptId, UserAttemptDto model);
    }
}
