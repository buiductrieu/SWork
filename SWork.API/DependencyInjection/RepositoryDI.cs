using SWork.Repository.Repository;
using SWork.Repository.UnitOfWork;
using SWork.RepositoryContract.Interfaces;
using SWork.RepositoryContract.IUnitOfWork;


namespace SWork.API.DependencyInjection
{
    public static class RepositoryDI
    {
        public static IServiceCollection AddRepositoryDependencies(this IServiceCollection services)
        {

            //DI Helpers
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

            // DI Entities
            services.AddTransient<IJobCategoryRepository, JobCategoryRepository>();
            services.AddTransient<IJobRepository, JobRepository>();
            services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
            services.AddTransient<ITemplateResumeRepository, TemplateResumeRepository>();
            services.AddTransient<ResumeRepository, ResumeRepository>();
            services.AddTransient<IStudentRepository, StudentRepository>();

            // DI UnitOfWork
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
