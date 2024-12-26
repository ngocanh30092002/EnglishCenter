using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ILearningProcessRepository : IGenericRepository<LearningProcess>
    {
        Task<bool> ChangeStatusAsync(LearningProcess processModel, ProcessStatusEnum status);
        Task<bool> ChangeStartTimeAsync(LearningProcess processModel, DateTime dateTime);
        Task<bool> ChangeEndTimeAsync(LearningProcess processModel, DateTime dateTime);
        Task<List<long>> GetExamsProcessAsync(long enrollId, string classId);
    }
}
