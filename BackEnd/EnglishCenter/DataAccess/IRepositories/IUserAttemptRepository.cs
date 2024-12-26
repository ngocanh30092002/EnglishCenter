using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IUserAttemptRepository : IGenericRepository<UserAttempt>
    {
        public Task<bool> ChangeUserAsync(UserAttempt model, string userId);
        public Task<bool> ChangeToeicAsync(UserAttempt model, long toeicId);
        public Task<bool> ChangeRoadMapExamAsync(UserAttempt model, long roadMapExamId);
        public Task<bool> ChangeListeningScoreAsync(UserAttempt model, int score);
        public Task<bool> ChangeReadingScoreAsync(UserAttempt model, int score);
    }
}
