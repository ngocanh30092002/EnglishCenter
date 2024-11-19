using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IToeicPracticeRecordRepository : IGenericRepository<ToeicPracticeRecord>
    {
        public Task<List<ToeicPracticeRecord>> GetResultAsync(long attemptId);
        public Task<int> GetNumCorrectRecordsWithPartAsync(long attemptId, int partNum);
        public Task<bool> ChangeSelectedAnswerAsync(ToeicPracticeRecord record, string? selectedAnswer);

    }
}
