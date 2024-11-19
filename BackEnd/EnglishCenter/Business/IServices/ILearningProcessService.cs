using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global.Enum;
using EnglishCenter.Presentation.Models;
using EnglishCenter.Presentation.Models.DTOs;

namespace EnglishCenter.Business.IServices
{
    public interface ILearningProcessService
    {
        public Task<Response> GetAllAsync();
        public Task<Response> GetAsync(long id);
        public Task<Response> GetHisProcessesAsync(long enrollId, long? assignmentId, long? examId);
        public Task<Response> GetOngoingAsync(long enrollId, long? assignmentId, long? examId);
        public Task<Response> GetScoreByProcessAsync(long id);
        public Task<Response> GetNumberAttemptedAsync(long enrollId, long assignmentId);
        public Task<Response> GetStatusExamAsync(long enrollId, long examId);
        public Task<Response> GetStatusLessonAsync(long id, long? assignmentId, long? examId);
        public Task<Response> IsSubmittedAsync(long id);
        public Task<LessonStatusEnum> IsStatusLessonAsync(Enrollment enroll, long? assignmentId, long? examId);
        public Task<Response> HandleSubmitProcessAsync(long id, LearningProcessDto model);
        public Task<Response> ChangeStatusAsync(long id, int status);
        public Task<Response> ChangeStartTimeAsync(long id, string dateTime);
        public Task<Response> ChangeEndTimeAsync(long id, string dateTime);
        public Task<Response> CreateAsync(LearningProcessDto model);
        public Task<Response> UpdateAsync(long id, LearningProcessDto model);
        public Task<Response> DeleteAsync(long id);
    }
}
