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
        IClassRepository Classes { get; }
        ICourseRepository Courses { get; }
        ICourseContentRepository CourseContents { get; }
        IAssignmentRepository Assignments { get; }
        IScoreHistoryRepository ScoreHis { get; }
        IAssignQuesRepository AssignQues { get; }
        IQuesLcImageRepository QuesLcImages { get; }
        IAnswerLcImageRepository AnswerLcImages { get; }
        IQuesLcAudioRepository QuesLcAudios { get; }
        IAnswerLcAudioRepository AnswerLcAudios { get; }
        IQuesLcConRepository QuesLcCons { get; }
        ISubLcConRepository SubLcCons { get; }
        IAnswerLcConRepository AnswerLcCons { get; }
        IQuesRcSentenceRepository QuesRcSentences { get; }
        IAnswerRcSentenceRepository AnswerRcSentences { get; }
        IQuesRcSingleRepository QuesRcSingles { get; }
        ISubRcSingleRepository SubRcSingles { get; }
        IAnswerRcSingleRepository AnswerRcSingles { get; }
        IQuesRcDoubleRepository QuesRcDoubles { get; }
        ISubRcDoubleRepository SubRcDoubles { get; }
        IAnswerRcDoubleRepository AnswerRcDoubles { get; }
        IQuesRcTripleRepository QuesRcTriples { get; }
        ISubRcTripleRepository SubRcTriples { get; }
        IAnswerRcTripleRepository AnswerRcTriples { get; }
        ILearningProcessRepository LearningProcesses { get; }
        IAssignmentRecordRepository AssignmentRecords { get; }
        IHomeworkRepository Homework { get; }
        IHomeQuesRepository HomeQues { get; }
        IHwSubmissionRepository HwSubmissions { get; }
        IHwSubRecordRepository HwSubRecords { get; }
        IToeicConversionRepository ToeicConversion { get; }
        IToeicRecordRepository ToeicRecords { get; }
        IExaminationRepository Examinations { get; }
        IToeicExamRepository ToeicExams { get; }
        IQuesToeicRepository QuesToeic { get; }
        ISubToeicRepository SubToeic { get; }
        IAnswerToeicRepository AnswerToeic { get; }
        IToeicDirectionRepository ToeicDirections { get; }
        IAttemptRecordRepository AttemptRecords { get; }
        IUserAttemptRepository UserAttempts { get; }
        IChatFileRepository ChatFiles { get; }
        IChatMessageRepository ChatMessages { get; }
        IPeriodRepository Periods { get; }
        IClassRoomRepository ClassRooms { get; }
        ILessonRepository Lessons { get; }
        IClassScheduleRepository ClassSchedules { get; }
        IClassMaterialRepository ClassMaterials { get; }
        ISubmissionTaskRepository SubmissionTasks { get; }
        ISubmissionFileRepository SubmissionFiles { get; }
        IUserWordRepository UserWords { get; }
        IRoadMapRepository RoadMaps { get; }
        IRoadMapExamRepository RoadMapExams { get; }
        IRandomQueToeicRepository RandomQues { get; }
        IAttendanceRepository Attendances { get; }
        INotificationRepository Notifications { get; }
        IIssueReportRepository IssueReports { get; }
        IIssueResponseRepository IssueResponses { get; }
        public Task<int> CompleteAsync();
        public Task BeginTransAsync();
        public Task CommitTransAsync();
        public Task RollBackTransAsync();
    }
}
