using System.Reflection;
using EnglishCenter.DataAccess.IRepositories;
using EnglishCenter.DataAccess.Repositories;
using EnglishCenter.DataAccess.UnitOfWork;
using EnglishCenter.Presentation.Helpers;

namespace EnglishCenter.Presentation.Extensions.Repository
{
    public static class RepositoryService
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var repos = assembly.GetTypes()
                            .Where(t => t.IsClass &&
                                        !t.IsAbstract &&
                                        t.BaseType != null &&
                                        t.BaseType.IsGenericType &&
                                        t.BaseType.GetGenericTypeDefinition() == typeof(GenericRepository<>))
                            .ToList();

            foreach (var repo in repos)
            {
                var interfaces = repo.GetInterfaces().Where(i => i.Name == $"I{repo.Name}");

                foreach (var @interface in interfaces )
                {
                    if(!@interface.IsGenericType  || @interface.GetGenericTypeDefinition() != typeof(IGenericRepository<>))
                    {
                        services.AddScoped(@interface, repo);
                    }
                }
            }

            services.AddScoped<MailHelper>();

            return services;
        }

        public static IServiceCollection AddServicesLayer(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var serviceTypes = Assembly.GetExecutingAssembly()
                                    .GetTypes()
                                    .Where(type => type.IsClass && !type.IsAbstract && type.Namespace != null && type.Namespace.Contains("EnglishCenter.Business") && type.Name.EndsWith("Service"))
                                    .ToList();

            foreach (var serviceType in serviceTypes)
            {
                var interfaces = serviceType.GetInterfaces().Where(i => i.Name == $"I{serviceType.Name}");
                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, serviceType);
                }
            }


            return services;
        }
    }
}
