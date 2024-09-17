using EnglishCenter.Helpers;
using EnglishCenter.Repositories;
using EnglishCenter.Repositories.AdminRepositories;
using EnglishCenter.Repositories.AuthenticationRepositories;
using EnglishCenter.Repositories.CourseRepositories;
using EnglishCenter.Repositories.HomeRepositories;
using EnglishCenter.Repositories.IRepositories;

namespace EnglishCenter.Extensions.Repository
{
    public static class RepositoryService
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ICourseContentRepository, CourseContentRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IEnrollStatusRepository, EnrollStatusRepository>();
            services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            services.AddScoped<IJsonWebTokenRepository, JsonWebTokenRepository>();
            services.AddScoped<IExternalLoginRepository, ExternalLoginRepository>();
            services.AddScoped<MailHelper>();
            return services;
        }
    }
}
