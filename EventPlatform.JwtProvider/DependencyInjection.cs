using EventPlatform.Application.Interfaces;
using EventPlatform.Jwt.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Jwt
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthTokenService, JwtTokenService>();
            return services;
        }
    }
}
