using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class SubmissionTaskRepository : GenericRepository<SubmissionTask>, ISubmissionTaskRepository
    {
        public SubmissionTaskRepository(EnglishCenterContext context) : base(context)
        {
        }

        public Task<bool> ChangeDescriptionAsync(SubmissionTask task, string newDescription)
        {
            if (task == null) return Task.FromResult(false);

            task.Description = newDescription;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeEndTimeAsync(SubmissionTask task, DateTime endTime)
        {
            if (task == null) return false;

            if (task.StartTime > endTime) return false;

            var files = await context.SubmissionFiles
                                     .Where(f => f.SubmissionTaskId == task.SubmissionId)
                                     .ToListAsync();

            foreach (var file in files)
            {
                if (task.StartTime <= file.UploadAt && file.UploadAt <= task.EndTime)
                {
                    file.Status = (int)SubmissionFileEnum.OnTime;
                }
                else
                {
                    file.Status = (int)SubmissionFileEnum.Late;
                }
            }

            task.EndTime = endTime;

            return true;
        }

        public async Task<bool> ChangeLessonAsync(SubmissionTask task, long lessonId)
        {
            if (task == null) return false;

            var lessonModel = await context.Lessons.FindAsync(lessonId);
            if (lessonModel == null) return false;

            task.Lesson = lessonModel;

            return true;
        }

        public async Task<bool> ChangeStartTimeAsync(SubmissionTask task, DateTime startTime)
        {
            if (task == null) return false;

            if (task.EndTime < startTime) return false;

            var files = await context.SubmissionFiles
                                    .Where(f => f.SubmissionTaskId == task.SubmissionId)
                                    .ToListAsync();

            foreach (var file in files)
            {
                if (task.StartTime <= file.UploadAt && file.UploadAt <= task.EndTime)
                {
                    file.Status = (int)SubmissionFileEnum.OnTime;
                }
                else
                {
                    file.Status = (int)SubmissionFileEnum.Late;
                }
            }

            task.StartTime = startTime;

            return true;
        }

        public Task<bool> ChangeTitleAsync(SubmissionTask task, string newTitle)
        {
            if (task == null) return Task.FromResult(false);

            task.Title = newTitle;

            return Task.FromResult(true);
        }
    }
}
