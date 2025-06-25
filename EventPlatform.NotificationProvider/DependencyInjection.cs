using EventPlatform.Application.Interfaces;
using EventPlatform.Notification.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventPlatform.NotificationProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificationService, EmailNotificationService>();
            return services;
        }
    }
}
