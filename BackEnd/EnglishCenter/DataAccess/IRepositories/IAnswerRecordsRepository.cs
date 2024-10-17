using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAnswerRecordsRepository : IGenericRepository<AnswerRecord>
    {
        public Task<bool> ChangeProcessAsync(AnswerRecord record, long processId);
        public Task<bool> ChangeAssignQuesAsync(AnswerRecord record, long assignQueId);
        public Task<bool> ChangeSubAsync(AnswerRecord record, long? subId);
        public Task<bool> ChangeSelectedAnswerAsync(AnswerRecord record, string selectedAnswer);
        public Task<bool> ChangeCorrectAsync(AnswerRecord record, bool isCorrect);
        public Task<bool> IsExistSubAsync(QuesTypeEnum type, long? subId);
    }
}
