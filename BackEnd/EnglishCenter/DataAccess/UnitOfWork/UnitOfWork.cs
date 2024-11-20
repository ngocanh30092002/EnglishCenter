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
            IAnswerLcAudioRepository answerLcAudioRepo,
            IQuesLcConRepository quesLcConRepo,
            ISubLcConRepository subLcConRepo,
            IAnswerLcConRepository answerLcConRepo,
            IQuesRcSentenceRepository quesRcSentenceRepo,
            IAnswerRcSentenceRepository answerRcSentenceRepo,
            IQuesRcSingleRepository quesRcSingleRepo,
            ISubRcSingleRepository subRcSingleRepo,
            IAnswerRcSingleRepository answerRcSingleRepo,
            IQuesRcDoubleRepository quesRcDoubleRepo,
            ISubRcDoubleRepository subRcDoubleRepo,
            IAnswerRcDoubleRepository answerRcDoubleRepo,
            IQuesRcTripleRepository quesRcTripleRepo,
            ISubRcTripleRepository subRcTripleRepo,
            IAnswerRcTripleRepository answerRcTripleRepo,
            ILearningProcessRepository learningProcessRepo,
            IAssignmentRecordRepository assignmentRecordRepo,
            IHomeworkRepository homeworkRepo,
            IHomeQuesRepository homeQuesRepo,
            IHwSubmissionRepository hwSubmissionRepo,
            IHwSubRecordRepository hwSubRecordRepo,
            IToeicConversionRepository toeicConRepo,
            IToeicRecordRepository toeicRecordRepo,
            IExaminationRepository examRepo,
            IToeicExamRepository toeicExamRepo,
            IQuesToeicRepository quesToeicRepo,
            ISubToeicRepository subToeicRepo,
            IAnswerToeicRepository answerToeicRepo,
            IToeicDirectionRepository toeicDirectionRepo,
            IToeicPracticeRecordRepository toeicPracticeRecordRepo,
            IToeicAttemptRepository toeicAttemptRepo,
            IChatMessageRepository chatMessageRepo,
            IChatFileRepository chatFileRepo,
            IPeriodRepository periodRepo,
            IClassRoomRepository classRoomRepo,
            ILessonRepository lessonRepo,
            IClassScheduleRepository classScheduleRepo,
            IClassMaterialRepository classMaterialRepo
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
            QuesLcImages = quesLcImageRepo;
            AnswerLcImages = answerLcImageRepo;
            AnswerLcAudios = answerLcAudioRepo;
            QuesLcAudios = quesLcAudioRepo;
            QuesLcCons = quesLcConRepo;
            SubLcCons = subLcConRepo;
            AnswerLcCons = answerLcConRepo;
            AnswerRcSentences = answerRcSentenceRepo;
            QuesRcSentences = quesRcSentenceRepo;
            QuesRcSingles = quesRcSingleRepo;
            SubRcSingles = subRcSingleRepo;
            AnswerRcSingles = answerRcSingleRepo;
            QuesRcDoubles = quesRcDoubleRepo;
            SubRcDoubles = subRcDoubleRepo;
            AnswerRcDoubles = answerRcDoubleRepo;
            QuesRcTriples = quesRcTripleRepo;
            AnswerRcTriples = answerRcTripleRepo;
            SubRcTriples = subRcTripleRepo;
            LearningProcesses = learningProcessRepo;
            AssignmentRecords = assignmentRecordRepo;
            Homework = homeworkRepo;
            HomeQues = homeQuesRepo;
            HwSubmissions = hwSubmissionRepo;
            HwSubRecords = hwSubRecordRepo;
            ToeicConversion = toeicConRepo;
            ToeicRecords = toeicRecordRepo;
            Examinations = examRepo;
            ToeicExams = toeicExamRepo;
            QuesToeic = quesToeicRepo;
            SubToeic = subToeicRepo;
            AnswerToeic = answerToeicRepo;
            ToeicDirections = toeicDirectionRepo;
            ToeicPracticeRecords = toeicPracticeRecordRepo;
            ToeicAttempts = toeicAttemptRepo;
            ChatFiles = chatFileRepo;
            ChatMessages = chatMessageRepo;
            Periods = periodRepo;
            ClassRooms = classRoomRepo;
            Lessons = lessonRepo;
            ClassSchedules = classScheduleRepo;
            ClassMaterials = classMaterialRepo;
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
        public IQuesLcImageRepository QuesLcImages { get; private set; }
        public IAnswerLcImageRepository AnswerLcImages { get; private set; }
        public IAnswerLcAudioRepository AnswerLcAudios { get; private set; }
        public IQuesLcAudioRepository QuesLcAudios { get; private set; }
        public IAnswerRcSentenceRepository AnswerRcSentences { get; private set; }
        public IQuesRcSentenceRepository QuesRcSentences { get; private set; }
        public IQuesLcConRepository QuesLcCons { get; private set; }
        public IAnswerLcConRepository AnswerLcCons { get; private set; }
        public ISubLcConRepository SubLcCons { get; private set; }
        public IQuesRcSingleRepository QuesRcSingles { get; private set; }
        public IAnswerRcSingleRepository AnswerRcSingles { get; private set; }
        public ISubRcSingleRepository SubRcSingles { get; private set; }
        public IQuesRcDoubleRepository QuesRcDoubles { get; private set; }
        public IAnswerRcDoubleRepository AnswerRcDoubles { get; private set; }
        public ISubRcDoubleRepository SubRcDoubles { get; private set; }
        public IQuesRcTripleRepository QuesRcTriples { get; private set; }
        public IAnswerRcTripleRepository AnswerRcTriples { get; private set; }
        public ISubRcTripleRepository SubRcTriples { get; private set; }
        public ILearningProcessRepository LearningProcesses { get; private set; }
        public IAssignmentRecordRepository AssignmentRecords { get; private set; }
        public IHomeworkRepository Homework { get; private set; }
        public IHomeQuesRepository HomeQues { get; private set; }
        public IHwSubmissionRepository HwSubmissions { get; private set; }
        public IHwSubRecordRepository HwSubRecords { get; private set; }
        public IToeicConversionRepository ToeicConversion { get; private set; }
        public IToeicRecordRepository ToeicRecords { get; private set; }
        public IExaminationRepository Examinations { get; private set; }
        public IToeicExamRepository ToeicExams { get; private set; }
        public IQuesToeicRepository QuesToeic { get; private set; }
        public ISubToeicRepository SubToeic { get; private set; }
        public IAnswerToeicRepository AnswerToeic { get; private set; }
        public IToeicDirectionRepository ToeicDirections { get; private set; }
        public IToeicPracticeRecordRepository ToeicPracticeRecords { get; private set; }
        public IToeicAttemptRepository ToeicAttempts { get; private set; }
        public IChatFileRepository ChatFiles { get; private set; }
        public IChatMessageRepository ChatMessages { get; private set; }
        public IPeriodRepository Periods { get; private set; }
        public IClassRoomRepository ClassRooms { get; private set; }
        public ILessonRepository Lessons { get; private set; }
        public IClassScheduleRepository ClassSchedules { get; private set; }
        public IClassMaterialRepository ClassMaterials { get; private set; }

        public async Task BeginTransAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransAsync()
        {
            if (_transaction != null)
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
