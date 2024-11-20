using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IClassScheduleRepository : IGenericRepository<ClassSchedule>
    {
        public Task<bool> ChangeStartPeriodAsync(ClassSchedule schedule, int start);
        public Task<bool> ChangeEndPeriodAsync(ClassSchedule schedule, int end);
        public Task<bool> ChangeDayOfWeekAsync(ClassSchedule schedule, int dayOfWeek);
        public Task<bool> ChangeClassAsync(ClassSchedule schedule, string classId);
        public Task<bool> ChangeClassRoomAsync(ClassSchedule schedule, long classRoomId);
        public Task<bool> IsDuplicateAsync(int dayOfWeek, long classRoomId, int start, int end, long? scheduleId = null);
    }
}
