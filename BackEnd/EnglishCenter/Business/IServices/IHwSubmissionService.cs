using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IHwSubmissionService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long hwSubId);
        public Task<Response> GetByEnrollAsync(long enrollId, long homeworkId);
        public Task<Response> GetOngoingAsync(long enrollId, long homeworkId);
        public Task<Response> GetNumberAttemptAsync(long enrollId, long homeworkId);
        public Task<Response> GetResultAsync(long id);
        public Task<Response> GetScoreAsync(long hwSubId);
        public Task<Response> IsSubmittedAsync(long hwSubId);
        public Task<bool> IsInChargeAsync(string userId, long subId);
        public Task<Response> CreateAsync(HwSubmissionDto model);
        public Task<Response> UpdateAsync(long hwSubId, HwSubmissionDto model);
        public Task<Response> HandleSubmitHomework(long hwSubId, HwSubmissionDto model);
        public Task<Response> DeleteAsync(long hwSubId);
        public Task<Response> ChangeHomeworkAsync(long hwSubId, long homeworkId);
        public Task<Response> ChangeDateAsync(long hwSubId, string dateTime);
        public Task<Response> ChangeFeedbackAsync(long hwSubId, string feedback);
        public Task<Response> ChangeEnrollAsync(long hwSubId, long enrollId);
        public Task<Response> ChangeIsPassAsync(long hwSubId, bool isPass);
    }
}
