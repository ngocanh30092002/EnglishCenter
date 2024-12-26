using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.Repositories.ClassRepositories
{
    public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(EnglishCenterContext context) : base(context)
        {

        }

        public Task<bool> ChangeIsAttendedAsync(Attendance attendance, bool isAttended)
        {
            if (attendance == null) return Task.FromResult(false);

            attendance.IsAttended = isAttended;

            if (isAttended)
            {
                attendance.IsPermitted = false;
                attendance.IsLate = false;
                attendance.IsLeaved = false;
            }
            else
            {
                attendance.IsLate = false;
            }

            return Task.FromResult(true);
        }

        public Task<bool> ChangeIsLateAsync(Attendance attendance, bool isLate)
        {
            if (attendance == null) return Task.FromResult(false);

            attendance.IsLate = isLate;

            if (isLate)
            {
                attendance.IsAttended = true;
                attendance.IsLeaved = false;
                attendance.IsPermitted = false;
                attendance.IsLeaved = false;
            }

            return Task.FromResult(true);
        }

        public Task<bool> ChangeIsLeavedAsync(Attendance attendance, bool isLeaved)
        {
            if (attendance == null) return Task.FromResult(false);

            attendance.IsLeaved = isLeaved;

            if (isLeaved)
            {
                attendance.IsAttended = false;
                attendance.IsLate = false;
            }
            else
            {
                attendance.IsPermitted = false;
            }

            return Task.FromResult(true);
        }

        public Task<bool> ChangeIsPermittedAsync(Attendance attendance, bool isPermitted)
        {
            if (attendance == null) return Task.FromResult(false);

            attendance.IsPermitted = isPermitted;

            if (isPermitted)
            {
                attendance.IsAttended = false;
                attendance.IsLate = false;
                attendance.IsLeaved = true;
            }

            return Task.FromResult(true);
        }
    }
}
