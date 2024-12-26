using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAttemptRecordRepository : IGenericRepository<AttemptRecord>
    {
        public Task<List<AttemptRecord>> GetResultAsync(long attemptId);
        public Task<int> GetNumCorrectRecordsWithPartAsync(long attemptId, int partNum);
        public Task<bool> ChangeSelectedAnswerAsync(AttemptRecord record, string? selectedAnswer);
        public Task<List<QuesToeic>?> GetByRoadMapQuestionAsync(long attemptId);
    }
}
