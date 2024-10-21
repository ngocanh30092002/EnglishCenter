using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IHwSubmissionRepository : IGenericRepository<HwSubmission>
    {
        public Task<bool> ChangeHomeworkAsync(HwSubmission submitModel, long homeworkId);
        public Task<bool> ChangeDateAsync(HwSubmission submitModel, DateTime dateTime);
        public Task<bool> ChangeStatusAsync(HwSubmission submitModel, SubmitStatusEnum status);
        public Task<bool> ChangeFeedbackAsync(HwSubmission submitModel, string feedback);
        public Task<bool> ChangeEnrollAsync(HwSubmission submitModel, long enrollId);
        public Task<bool> ChangeIsPassAsync(HwSubmission submitModel, bool isPass); 
    }
}
