using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.Repositories.CourseRepositories
{
    public class ScoreHistoryRepository : GenericRepository<ScoreHistory>, IScoreHistoryRepository
    {
        public ScoreHistoryRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeEntrancePointAsync(ScoreHistory model, int score)
        {
            if (model == null) return Task.FromResult(false);
            if (score < 0) return Task.FromResult(false);

            model.EntrancePoint = score;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeFinalPointAsync(ScoreHistory model, int score)
        {
            if (model == null) return Task.FromResult(false);
            if (score < 0) return Task.FromResult(false);

            model.FinalPoint = score;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeMidtermPointAsync(ScoreHistory model, int score)
        {
            if (model == null) return Task.FromResult(false);
            if (score < 0) return Task.FromResult(false);

            model.MidtermPoint = score;
            return Task.FromResult(true);
        }

        public async Task<Response> UpdateAsync(long scoreId, ScoreHistoryDto model)
        {
            var scoreModel = await context.ScoreHistories.FindAsync(scoreId);
            
            if (scoreModel == null)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't find any ScoreHistory"
                };
            }

            var isChangeSuccess = await this.ChangeEntrancePointAsync(scoreModel, model.EntrancePoint ?? 0);

            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't change Entrance Point"
                };
            }

            isChangeSuccess = await this.ChangeMidtermPointAsync(scoreModel, model.MidtermPoint ?? 0);

            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't change Midterm Point"
                };
            }

            isChangeSuccess = await this.ChangeFinalPointAsync(scoreModel, model.FinalPoint ?? 0);
            if (!isChangeSuccess)
            {
                return new Response()
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Can't change Midterm Point"
                };
            }

            return new Response()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
            };
        }
    }
}
