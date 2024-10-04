using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.IRepositories;

namespace EnglishCenter.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnglishCenterContext _context;
        public UnitOfWork(
            EnglishCenterContext context,
            IEventRepository eventRepo,
            IStudentRepository studentRepo,
            ITeacherRepository teacherRepo,
            IEnrollStatusRepository enrollStatusRepo,
            IEnrollmentRepository enrollRepo,
            IClassRepository classRepo,
            ICourseRepository courseRepo,
            ICourseContentRepository courseContentRepo,
            IAssignmentRepository assignRepo,
            IScoreHistoryRepository scoreHisRepo,
            IAssignQuesRepository assignQues
            ) 
        {
            _context = context;
            Events = eventRepo;
            Students = studentRepo;
            Teachers = teacherRepo;
            EnrollStatus = enrollStatusRepo;
            Enrollment = enrollRepo;
            Classes = classRepo;
            Courses = courseRepo;
            CourseContents = courseContentRepo;
            Assignments = assignRepo;
            ScoreHis = scoreHisRepo;
            AssignQues = assignQues;

        }

        public IEventRepository Events { get; private set; }
        public IStudentRepository Students { get; private set; }
        public ITeacherRepository Teachers { get; private set; }
        public IEnrollStatusRepository EnrollStatus { get; private set; }
        public IEnrollmentRepository Enrollment { get; private set; }
        public IClassRepository Classes { get; private set; }
        public ICourseRepository Courses { get; private set; }
        public ICourseContentRepository CourseContents { get; private set; }
        public IAssignmentRepository Assignments { get; private set; }
        public IScoreHistoryRepository ScoreHis { get; private set; }
        public IAssignQuesRepository AssignQues { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
