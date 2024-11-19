using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAssignmentRecordRepository : IGenericRepository<AssignmentRecord>
    {
        public Task<bool> ChangeProcessAsync(AssignmentRecord record, long processId);
        public Task<bool> ChangeAssignQuesAsync(AssignmentRecord record, long assignQueId);
        public Task<bool> ChangeSubAsync(AssignmentRecord record, long? subId);
        public Task<bool> ChangeSelectedAnswerAsync(AssignmentRecord record, string selectedAnswer);
        public Task<bool> IsExistSubAsync(QuesTypeEnum type, long? subId);
    }
}
