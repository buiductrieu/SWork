using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWork.Common.Helper;
using SWork.Repository.Repository;
using SWork.RepositoryContract.Interfaces;

namespace SWork.Repository
{
    public static class DependencyInjcection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            // DI UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
        
    }
}
