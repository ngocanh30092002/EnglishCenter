using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EnglishCenter.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EnglishCenterContext _context;
        private IDbContextTransaction? _transaction;

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
            IAssignQuesRepository assignQuesRepo,
            IQuesLcImageRepository quesLcImageRepo,
            IAnswerLcImageRepository answerLcImageRepo,
            IQuesLcAudioRepository quesLcAudioRepo,
            IAnswerLcAudioRepository answerLcAudioRepo
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
            AssignQues = assignQuesRepo;
            QuesLcImage = quesLcImageRepo;
            AnswerLcImage = answerLcImageRepo;
            AnswerLcAudio = answerLcAudioRepo;
            QuesLcAudio = quesLcAudioRepo;
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
        public IQuesLcImageRepository QuesLcImage { get; private set; }
        public IAnswerLcImageRepository AnswerLcImage { get; private set; }
        public IAnswerLcAudioRepository AnswerLcAudio { get; private set; }
        public IQuesLcAudioRepository QuesLcAudio { get; private set; }

        public async Task BeginTransAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransAsync()
        {
            if(_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollBackTransAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

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
