using EnglishCenter.Database;
using EnglishCenter.Global;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Extensions.Database
{
    public static class DatabaseContextServices
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<EnglishCenterContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString(GlobalVariable.DATABASE));
            });

            return services;
        }
    }
}
