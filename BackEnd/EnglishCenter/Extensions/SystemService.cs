namespace EnglishCenter.Extensions
{
    public static class SystemService
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(builder.Configuration["JWT:ValidAudience"]);
                });
            });

            return services;
        }
    }
}
