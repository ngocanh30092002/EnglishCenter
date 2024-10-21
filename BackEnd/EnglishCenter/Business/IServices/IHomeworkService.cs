using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface IHomeworkService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> ChangeClassAsync(long id, string classId);
        public Task<Response> ChangeStartTimeAsync(long id, string startTime);
        public Task<Response> ChangeEndTimeAsync(long id, string endTime);
        public Task<Response> ChangeLateSubmitDaysAsync(long id, int days);
        public Task<Response> ChangePercentageAsync(long id, int percentage);
        public Task<Response> ChangeTitleAsync(long id, string title);
        public Task<Response> ChangeTimeAsync(long id, string time);
        public Task<Response> CreateAsync(HomeworkDto model);
        public Task<Response> UpdateAsync(long id, HomeworkDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
