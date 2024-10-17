using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ILearningProcessService 
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> HandleSubmitProcessAsync(long id, LearningProcessDto model);
        public Task<Response> ChangeStatusAsync(long id, int status);
        public Task<Response> ChangeStartTimeAsync(long id, string dateTime);
        public Task<Response> ChangeEndTimeAsync(long id, string dateTime);
        public Task<Response> CreateAsync(LearningProcessDto model);
        public Task<Response> UpdateAsync(long id, LearningProcessDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
