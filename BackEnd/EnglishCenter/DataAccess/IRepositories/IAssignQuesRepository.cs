using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAssignQuesRepository :  IGenericRepository<AssignQue>
    {
        Task<bool> LoadQuestionAsync(AssignQue model);
        Task<List<AssignQue>?> GetByAssignmentAsync(long assignmentId);
        Task<bool> IsExistQuesIdAsync(QuesTypeEnum type, long quesId);
        Task<bool> ChangeQuesAsync(AssignQue model, QuesTypeEnum type, long quesId);
        Task<bool> ChangeAssignmentIdAsync(AssignQue model, long assignmentId);
        Task<bool> ChangeNoNumAsync(AssignQue model, int noNum);
        Task<TimeOnly> GetTimeQuesAsync(AssignQue model);
        Task<bool> UpdateAsync(long id, AssignQueDto model);
    }
}
