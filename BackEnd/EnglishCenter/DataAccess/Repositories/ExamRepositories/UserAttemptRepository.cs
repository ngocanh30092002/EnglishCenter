using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ExamRepositories
{
    public class UserAttemptRepository : GenericRepository<UserAttempt>, IUserAttemptRepository
    {
        public UserAttemptRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeListeningScoreAsync(UserAttempt model, int score)
        {
            if (model == null) return Task.FromResult(false);

            model.ListeningScore = score;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeReadingScoreAsync(UserAttempt model, int score)
        {
            if (model == null) return Task.FromResult(false);

            model.ReadingScore = score;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeRoadMapExamAsync(UserAttempt model, long roadMapExamId)
        {
            var roadMapExamModel = await context.RoadMapExams.FindAsync(roadMapExamId);
            if (roadMapExamModel == null || model == null) return false;

            model.ToeicId = null;
            model.RoadMapExamId = roadMapExamId;

            return true;
        }

        public async Task<bool> ChangeToeicAsync(UserAttempt model, long toeicId)
        {
            var toeicModel = await context.ToeicExams.FindAsync(toeicId);
            if (toeicModel == null || model == null) return false;

            model.ToeicId = toeicId;
            model.RoadMapExamId = null;

            return true;
        }

        public async Task<bool> ChangeUserAsync(UserAttempt model, string userId)
        {
            if (model == null) return false;

            var userModel = await context.Users.FindAsync(userId);
            if (userModel == null) return false;

            model.UserId = userId;

            return true;
        }
    }
}
