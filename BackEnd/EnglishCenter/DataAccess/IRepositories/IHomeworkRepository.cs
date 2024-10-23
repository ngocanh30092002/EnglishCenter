using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IHomeworkRepository : IGenericRepository<Homework>
    {
        public Task<bool> IsInChargeAsync(Homework homework, string userId);
        public Task<bool> ChangePercentageAsync(Homework homeModel, int percentage);
        public Task<bool> ChangeTitleAsync(Homework homeModel, string title);
        public Task<bool> ChangeTimeAsync(Homework homeModel, TimeOnly time);
        public Task<bool> ChangeClassAsync(Homework homeModel, string classId);
        public Task<bool> ChangeStartTimeAsync(Homework homeModel, DateTime startTime);
        public Task<bool> ChangeEndTimeAsync(Homework homeModel, DateTime endTime);
        public Task<bool> ChangeLateSubmitDaysAsync(Homework homeModel, int days);
    }
}
