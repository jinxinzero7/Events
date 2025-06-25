using EventPlatform.Application.Interfaces;
using EventPlatform.Payments.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventPlatform.Payments
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPaymentService, YooKassaPaymentService>();
            return services;
        }
    }
}
