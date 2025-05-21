using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWork.Common.Helper;
using SWork.Service.Services;
using SWork.ServiceContract.Interfaces;


namespace SWork.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<APIResponse>();
            return services;
        }
    }
}
