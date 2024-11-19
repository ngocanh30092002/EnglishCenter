using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IToeicAttemptRepository : IGenericRepository<ToeicAttempt>
    {
        public Task<bool> ChangeUserAsync(ToeicAttempt model, string userId);
        public Task<bool> ChangeToeicAsync(ToeicAttempt model, long toeicId);
        public Task<bool> ChangeListeningScoreAsync(ToeicAttempt model, int score);
        public Task<bool> ChangeReadingScoreAsync(ToeicAttempt model, int score);
    }
}
