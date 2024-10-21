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
        IAnswerRecordsRepository AnswerRecords { get; }
        IHomeworkRepository Homework { get; }
        IHomeQuesRepository HomeQues { get; }
        IHwSubmissionRepository HwSubmissions { get; }
        IHwSubRecordRepository HwSubRecords { get; }
        
        public Task<int> CompleteAsync();
        public Task BeginTransAsync();
        public Task CommitTransAsync();
        public Task RollBackTransAsync();
    }
}
