
using SWork.Common.Helper;
//using SWork.Service.CloudinaryService;
using SWork.Service.Services;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.DependencyInjection
{
    public static class ServiceDI
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<APIResponse>();
            return services;
        }

    }
}
