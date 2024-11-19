using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class ToeicAttemptRepository : GenericRepository<ToeicAttempt>, IToeicAttemptRepository
    {
        public ToeicAttemptRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeListeningScoreAsync(ToeicAttempt model, int score)
        {
            if (model == null) return Task.FromResult(false);

            model.ListeningScore = score;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeReadingScoreAsync(ToeicAttempt model, int score)
        {
            if (model == null) return Task.FromResult(false);

            model.ReadingScore = score;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeToeicAsync(ToeicAttempt model, long toeicId)
        {
            var toeicModel = await context.ToeicExams.FindAsync(toeicId);
            if (toeicModel == null || model == null) return false;

            model.ToeicId = toeicId;

            return true;
        }

        public async Task<bool> ChangeUserAsync(ToeicAttempt model, string userId)
        {
            if (model == null) return false;

            var userModel = await context.Users.FindAsync(userId);
            if (userModel == null) return false;

            model.UserId = userId;

            return true;
        }
    }
}
