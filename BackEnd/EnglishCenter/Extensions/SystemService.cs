using EnglishCenter.Models;
using Microsoft.OpenApi.Models;

namespace EnglishCenter.Extensions
{
    public static class SystemService
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllPolicy",
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:5173")
                                             .AllowAnyMethod()
                                             .AllowAnyHeader()
                                             .AllowCredentials();
                                  });
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddRouting();
            builder.Services.AddHttpContextAccessor();
            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddSession();
            builder.Services.AddMvc();

            return services;
        }
    }
}
