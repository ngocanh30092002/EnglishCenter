using EnglishCenter.Business.IServices;
using EnglishCenter.Business.Services;
using EnglishCenter.Business.Services.Assignments;
using EnglishCenter.Business.Services.Authorization;
using EnglishCenter.Business.Services.Courses;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.DataAccess.Repositories;
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
            services.AddScoped<IAnswerLcAudioRepository , AnswerLcAudioRepository>();
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
            return services;
        }
    }
}
