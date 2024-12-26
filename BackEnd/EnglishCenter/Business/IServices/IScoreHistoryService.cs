using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IScoreHistoryService
    {
        public Task<Response> CreateAsync(ScoreHistoryDto model);
        public Task<Response> UpdateAsync(long scoreId, ScoreHistoryDto model);
        public Task<Response> DeleteAsync(long scoreId);
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long scoreId);
        public Task<Response> GetByClassAsync(string classId);
        public Task<Response> ChangeEntrancePointAsync(long scoreId, int score);
        public Task<Response> ChangeMidtermPointAsync(long scoreId, int score);
        public Task<Response> ChangeFinalPointAsync(long scoreId, int score);
    }
}
