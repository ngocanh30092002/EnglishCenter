using EnglishCenter.DataAccess.Entities;

namespace EnglishCenter.DataAccess.IRepositories
{
    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        public Task<bool> ChangeIsAttendedAsync(Attendance attendance, bool isAttended);
        public Task<bool> ChangeIsPermittedAsync(Attendance attendance, bool isPermitted);
        public Task<bool> ChangeIsLateAsync(Attendance attendance, bool isLate);
        public Task<bool> ChangeIsLeavedAsync(Attendance attendance, bool isLeaved);
    }
}
