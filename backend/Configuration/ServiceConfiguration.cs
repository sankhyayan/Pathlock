using TaskManager.API.Core.Interfaces;
using TaskManager.API.Infrastructure.Persistence;
using TaskManager.API.Application.Interfaces;
using TaskManager.API.Application.Services;

namespace TaskManager.API.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register persistence services
            services.AddSingleton<IUserService, FileBasedUserService>();
            services.AddSingleton<IProjectService, FileBasedProjectService>();
            services.AddSingleton<ITaskService, FileBasedTaskService>();
            
            // Register application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISchedulingService, SchedulingService>();

            return services;
        }
    }
}
