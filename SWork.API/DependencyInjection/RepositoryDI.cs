using SWork.Common.Helper;
using SWork.Repository.Repository;
using SWork.Repository.UnitOfWork;
using SWork.RepositoryContract.Interfaces;
using SWork.RepositoryContract.IUnitOfWork;
using SWork.Service.Services;
using SWork.ServiceContract.Interfaces;

namespace SWork.API.DependencyInjection
{
    public static class RepositoryDI
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
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
