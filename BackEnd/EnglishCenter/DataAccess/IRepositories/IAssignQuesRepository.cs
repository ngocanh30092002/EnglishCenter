using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAssignQuesRepository :  IGenericRepository<AssignQue>
    {
        public Task<bool> LoadQuestionAsync(AssignQue model);
        public Task<bool> LoadQuestionWithoutAnswerAsync(AssignQue model);
        public Task<List<AssignQue>?> GetByAssignmentAsync(long assignmentId);
        public Task<bool> IsExistQuesIdAsync(QuesTypeEnum type, long quesId);
        public Task<bool> IsCorrectAnswerAsync(AssignQue model, string selectedAnswer, long? subId);
        public Task<bool> ChangeQuesAsync(AssignQue model, QuesTypeEnum type, long quesId);
        public Task<bool> ChangeAssignmentIdAsync(AssignQue model, long assignmentId);
        public Task<bool> ChangeNoNumAsync(AssignQue model, int noNum);
        public Task<TimeOnly> GetTimeQuesAsync(AssignQue model);
        public Task<int> GetNumberByAssignmentAsync(long assignmentId);
        public Task<bool> UpdateAsync(long id, AssignQueDto model);
    }
}
