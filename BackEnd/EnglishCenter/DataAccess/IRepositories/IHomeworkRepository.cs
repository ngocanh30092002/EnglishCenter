using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IHomeworkRepository : IGenericRepository<Homework>
    {
        public Task<bool> ChangeImageAsync(Homework homeModel, string newPath);
        public Task<bool> ChangeLessonAsync(Homework homeModel, long lessonId);
        public Task<bool> ChangePercentageAsync(Homework homeModel, int percentage);
        public Task<bool> ChangeTitleAsync(Homework homeModel, string title);
        public Task<bool> ChangeTimeAsync(Homework homeModel, TimeOnly time);
        public Task<bool> ChangeStartTimeAsync(Homework homeModel, DateTime startTime);
        public Task<bool> ChangeEndTimeAsync(Homework homeModel, DateTime endTime);
        public Task<bool> ChangeLateSubmitDaysAsync(Homework homeModel, int days);
        public Task<bool> IsInChargeAsync(Homework homework, string userId);
    }
}
