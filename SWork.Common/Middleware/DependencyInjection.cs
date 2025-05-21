using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace SWork.Common.Middleware
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
        {
            services.AddExceptionHandler<UnauthorizedExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddExceptionHandler<KeyNotFoundExceptionHandler>();
            services.AddExceptionHandler<NotImplementExceptionHandler>();
            services.AddExceptionHandler<BadRequestExceptionHandler>();
            services.AddExceptionHandler<EmailNotConfirmedExceptionHandler>();
            //services.AddSingleton<IAuthorizationHandler, IsFPTAdminHandler>();
            return services;
        }
    }
}
