using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface ISubmissionTaskRepository : IGenericRepository<SubmissionTask>
    {
        public Task<bool> ChangeTitleAsync(SubmissionTask task, string newTitle);
        public Task<bool> ChangeDescriptionAsync(SubmissionTask task, string newDescription);
        public Task<bool> ChangeStartTimeAsync(SubmissionTask task, DateTime startTime);
        public Task<bool> ChangeEndTimeAsync(SubmissionTask task, DateTime endTime);
        public Task<bool> ChangeLessonAsync(SubmissionTask task, long lessonId);
    }
}
