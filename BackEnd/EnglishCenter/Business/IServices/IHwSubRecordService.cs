using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IHwSubRecordService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetByHwSubmitAsync(long hwSubId);
        public Task<Response> ChangeSubmissionAsync(long id, long hwSubId);
        public Task<Response> ChangeHomeQuesAsync(long id, long homeQueId);
        public Task<Response> ChangeSubAsync(long id, long? subId);
        public Task<Response> ChangeSelectedAnswerAsync(long id, string selectedAnswer);
        public Task<Response> CreateAsync(HwSubRecordDto model);
        public Task<Response> UpdateAsync(long id, HwSubRecordDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
