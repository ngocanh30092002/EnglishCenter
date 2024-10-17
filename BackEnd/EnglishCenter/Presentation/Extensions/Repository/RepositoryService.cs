using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services;
using EnglishCenter.Business.Services.Assignments;
using EnglishCenter.Business.Services.Authorization;
using EnglishCenter.Business.Services.Courses;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.DataAccess.Repositories.AssignmentRepositories;
using EnglishCenter.DataAccess.Repositories.AuthenticationRepositories;
using EnglishCenter.DataAccess.Repositories.CourseRepositories;
using EnglishCenter.DataAccess.Repositories.HomeRepositories;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;

namespace EnglishCenter.Presentation.Extensions.Repository
{
    public static class RepositoryService
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICourseContentRepository, CourseContentRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IEnrollStatusRepository, EnrollStatusRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IScoreHistoryRepository, ScoreHistoryRepository>();
            services.AddScoped<IAssignQuesRepository, AssignQuesRepository>();
            services.AddScoped<IQuesLcImageRepository , QuesLcImageRepository>();
            services.AddScoped<IAnswerLcImageRepository, AnswerLcImageRepository>();
            services.AddScoped<IQuesLcAudioRepository, QuesLcAudioRepository>();
            services.AddScoped<IAnswerLcAudioRepository, AnswerLcAudioRepository>();
            services.AddScoped<IQuesRcSentenceRepository, QuesRcSentenceRepository>();
            services.AddScoped<IAnswerRcSentenceRepository, AnswerRcSentenceRepository>();
            services.AddScoped<IQuesLcConRepository, QuesLcConRepository>();
            services.AddScoped<ISubLcConRepository, SubLcConRepository>();
            services.AddScoped<IAnswerLcConRepository, AnswerLcConRepository>();
            services.AddScoped<IQuesRcSingleRepository, QuesRcSingleRepository>();
            services.AddScoped<ISubRcSingleRepository, SubRcSingleRepository>();
            services.AddScoped<IAnswerRcSingleRepository, AnswerRcSingleRepository>();
            services.AddScoped<IQuesRcDoubleRepository, QuesRcDoubleRepository>();
            services.AddScoped<ISubRcDoubleRepository, SubRcDoubleRepository>();
            services.AddScoped<IAnswerRcDoubleRepository, AnswerRcDoubleRepository>();
            services.AddScoped<IQuesRcTripleRepository, QuesRcTripleRepository>();
            services.AddScoped<ISubRcTripleRepository,  SubRcTripleRepository>();
            services.AddScoped<IAnswerRcTripleRepository, AnswerRcTripleRepository>();
            services.AddScoped<ILearningProcessRepository, LearningProcessRepository>();
            services.AddScoped<IAnswerRecordsRepository, AnswerRecordRepository>();
            services.AddScoped<MailHelper>();
            
            return services;
        }

        public static IServiceCollection AddServicesLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IExternalLoginService, ExternalLoginService>();
            services.AddScoped<IJsonTokenService, JsonTokenService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEnrollStatusService, EnrollStatusService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseContentService, CourseContentService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IScoreHistoryService, ScoreHistoryService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAssignQuesService, AssignQuesService>();
            services.AddScoped<IQuesLcImageService , QuesLcImageService>();
            services.AddScoped<IAnswerLcImageService, AnswerLcImageService>();
            services.AddScoped<IQuesLcAudioService, QuesLcAudioService>();
            services.AddScoped<IAnswerLcAudioService, AnswerLcAudioService>();
            services.AddScoped<IQuesRcSentenceService, QuesRcSentenceService>();
            services.AddScoped<IAnswerRcSentenceService, AnswerRcSentenceService>();
            services.AddScoped<IQuesLcConService, QuesLcConService>();
            services.AddScoped<ISubLcConService, SubLcConService>();
            services.AddScoped<IAnswerLcConService, AnswerLcConService>(); 
            services.AddScoped<IQuesRcSingleService, QuesRcSingleService>();
            services.AddScoped<ISubRcSingleService, SubRcSingleService>();
            services.AddScoped<IAnswerRcSingleService, AnswerRcSingleService>();
            services.AddScoped<IQuesRcDoubleService, QuesRcDoubleService>();
            services.AddScoped<ISubRcDoubleService, SubRcDoubleService>();
            services.AddScoped<IAnswerRcDoubleService, AnswerRcDoubleService>();
            services.AddScoped<IQuesRcTripleService, QuesRcTripleService>();
            services.AddScoped<ISubRcTripleService, SubRcTripleService>();
            services.AddScoped<IAnswerRcTripleService, AnswerRcTripleService>();
            services.AddScoped<ILearningProcessService, LearningProcessService>();
            services.AddScoped<IAnswerRecordService, AnswerRecordService>();

            return services;
        }
    }
}
