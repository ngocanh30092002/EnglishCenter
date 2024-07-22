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

            return services;
        }
    }
}
