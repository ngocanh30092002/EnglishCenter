using EnglishCenter.DataAccess.Database;
using EnglishCenter.Presentation.Global;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Presentation.Extensions.Database
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
