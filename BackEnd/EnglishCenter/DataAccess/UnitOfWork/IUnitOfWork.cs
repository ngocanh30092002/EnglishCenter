using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }    
        IEnrollStatusRepository EnrollStatus { get; }
        IEnrollmentRepository Enrollment { get; }
        IClassRepository Classes {  get; }
        ICourseRepository Courses { get; }
        ICourseContentRepository CourseContents { get; }
        IAssignmentRepository Assignments { get; }
        IScoreHistoryRepository ScoreHis { get; }
        public Task<int> CompleteAsync();
    }
}
