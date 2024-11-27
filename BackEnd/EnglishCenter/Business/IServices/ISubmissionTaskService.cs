using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ISubmissionTaskService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetCurrentByClassAsync(string classId);
        public Task<Response> ChangeTitleAsync(long id, string newTitle);
        public Task<Response> ChangeDescriptionAsync(long id, string newDescription);
        public Task<Response> ChangeStartTimeAsync(long id, string startTime);
        public Task<Response> ChangeEndTimeAsync(long id, string endTime);
        public Task<Response> ChangeLessonAsync(long id, long lessonId);
        public Task<Response> CreateAsync(SubmissionTaskDto model);
        public Task<Response> UpdateAsync(long id, SubmissionTaskDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
