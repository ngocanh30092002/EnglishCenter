using EnglishCenter.Helpers;
using EnglishCenter.Repositories;
using EnglishCenter.Repositories.AuthenticationRepositories;
using EnglishCenter.Repositories.IRepositories;

namespace EnglishCenter.Extensions.Repository
{
    public static class RepositoryService
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IClaimRepository, ClaimRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJsonWebTokenRepository, JsonWebTokenRepository>();
            services.AddScoped<IExternalLoginRepository, ExternalLoginRepository>();
            services.AddScoped<MailHelper>();
            return services;
        }
    }
}
