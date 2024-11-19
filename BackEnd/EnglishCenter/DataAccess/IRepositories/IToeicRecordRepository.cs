using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IToeicRecordRepository : IGenericRepository<ToeicRecord>
    {
        public Task<List<ToeicRecord>> GetResultAsync(long processId);
        public Task<bool> ChangeProcessAsync(ToeicRecord record, long processId);
        public Task<bool> ChangeSelectedAnswerAsync(ToeicRecord record, string? selectedAnswer);
        public Task<int> GetNumCorrectRecordsWithPartAsync(long processId, int partNum);

    }
}
