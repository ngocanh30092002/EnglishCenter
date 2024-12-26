using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.HomeworkRepositories
{
    public class HomeworkRepository : GenericRepository<Homework>, IHomeworkRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeworkRepository(EnglishCenterContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<bool> ChangeImageAsync(Homework homeModel, string newPath)
        {
            if (homeModel == null) return Task.FromResult(false);

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, newPath);

            if (!File.Exists(filePath)) return Task.FromResult(false);

            homeModel.Image = newPath;

            return Task.FromResult(true);
        }

        public async Task<bool> ChangeLessonAsync(Homework homeModel, long lessonId)
        {
            if (homeModel == null) return false;

            var lessonModel = await context.Lessons.FindAsync(lessonId);
            if (lessonModel == null) return false;

            homeModel.Lesson = lessonModel;

            return true;
        }

        public Task<bool> ChangeEndTimeAsync(Homework homeModel, DateTime endTime)
        {
            if (homeModel == null) return Task.FromResult(false);
            if (homeModel.StartTime > endTime) return Task.FromResult(false);

            homeModel.EndTime = endTime;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeLateSubmitDaysAsync(Homework homeModel, int days)
        {
            if (homeModel == null) return Task.FromResult(false);
            if (days < 0) return Task.FromResult(false);

            homeModel.LateSubmitDays = days;

            return Task.FromResult(true);
        }

        public Task<bool> ChangePercentageAsync(Homework homeModel, int percentage)
        {
            if (homeModel == null) return Task.FromResult(false);
            if (percentage < 0 || percentage > 100) return Task.FromResult(false);

            homeModel.AchievedPercentage = percentage;
            return Task.FromResult(true);
        }

        public Task<bool> ChangeStartTimeAsync(Homework homeModel, DateTime startTime)
        {
            if (homeModel == null) return Task.FromResult(false);
            if (homeModel.EndTime < startTime) return Task.FromResult(false);

            homeModel.StartTime = startTime;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTimeAsync(Homework homeModel, TimeOnly time)
        {
            if (homeModel == null) return Task.FromResult(false);

            homeModel.Time = time;

            return Task.FromResult(true);
        }

        public Task<bool> ChangeTitleAsync(Homework homeModel, string title)
        {
            if (homeModel == null) return Task.FromResult(false);

            homeModel.Title = title;

            return Task.FromResult(true);
        }

        public async Task<bool> IsInChargeAsync(Homework homework, string userId)
        {
            if (homework == null) return false;

            var lesson = await context.Lessons
                                      .Include(l => l.Class)
                                      .FirstOrDefaultAsync(l => l.LessonId == homework.LessonId);

            if (lesson == null) return false;


            return lesson.Class.TeacherId == userId;
        }
    }
}
