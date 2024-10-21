using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.Presentation.Global.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.DataAccess.Repositories.HomeworkRepositories
{
    public class HomeworkRepository : GenericRepository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(EnglishCenterContext context) : base(context)
        {
        }

        public async Task<bool> ChangeClassAsync(Homework homeModel, string classId)
        {
            if (homeModel == null) return false;

            var isExistClass = await context.Classes.AnyAsync(c => c.ClassId == classId && c.Status == (int) ClassEnum.Opening);
            if (!isExistClass) return false;

            homeModel.ClassId = classId;

            return true;
        }

        public Task<bool> ChangeEndTimeAsync(Homework homeModel, DateTime endTime)
        {
            if (homeModel == null) return Task.FromResult(false);
            if(homeModel.StartTime > endTime) return Task.FromResult(false);

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
            if(percentage < 0 || percentage > 100) return Task.FromResult(false);

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
    }
}
