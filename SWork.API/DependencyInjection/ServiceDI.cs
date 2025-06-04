
using SWork.Common.Helper;
using SWork.Data.Entities;
using SWork.Service.CloudinaryService;
using SWork.Service.Services;
using SWork.ServiceContract.ICloudinaryService;
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

            // DI service entities
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<IJobCategoryService, JobCategoryService>();
            services.AddScoped<IStudentService, StudentService>();
            //Cloudinary
            services.AddScoped<ICloudinaryImageService, CloudinaryImageService>();
            return services;
        }

    }
}
