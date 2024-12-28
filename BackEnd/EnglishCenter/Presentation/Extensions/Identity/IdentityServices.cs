using System.Text;
using EnglishCenter.DataAccess.Database;
using EnglishCenter.DataAccess.Entities;
using EnglishCenter.Presentation.Global;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace EnglishCenter.Presentation.Extensions.Identity
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(GlobalVariable.MAX_FAILED_ACCESS);
                options.Lockout.MaxFailedAccessAttempts = GlobalVariable.MAX_FAILED_ACCESS;
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<EnglishCenterContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
                    ClockSkew = TimeSpan.Zero,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access-token"];
                        return Task.CompletedTask;
                    }
                };

            })
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
            })
            .AddFacebook(options =>
            {
                options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("all-roles", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User.IsInRole(AppRole.ADMIN) ||
                               context.User.IsInRole(AppRole.STUDENT) ||
                               context.User.IsInRole(AppRole.TEACHER);
                    });
                });

                options.AddPolicy("admin-student", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User.IsInRole(AppRole.ADMIN) ||
                               context.User.IsInRole(AppRole.STUDENT);
                    });
                });

                options.AddPolicy("admin-teacher", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User.IsInRole(AppRole.ADMIN) ||
                               context.User.IsInRole(AppRole.TEACHER);
                    });
                });
            });

            return services;
        }
    }
}
