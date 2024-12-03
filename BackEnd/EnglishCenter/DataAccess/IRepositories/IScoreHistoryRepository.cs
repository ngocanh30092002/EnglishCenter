using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IScoreHistoryRepository : IGenericRepository<ScoreHistory>
    {
        public Task<Response> UpdateAsync(long scoreId, ScoreHistoryDto model);
        public Task<List<ScoreHistory>> GetByClassAsync(string classId);
        public Task<bool> ChangeEntrancePointAsync(ScoreHistory model, int score);
        public Task<bool> ChangeMidtermPointAsync(ScoreHistory model, int score);
        public Task<bool> ChangeFinalPointAsync(ScoreHistory model, int score);
    }
}
