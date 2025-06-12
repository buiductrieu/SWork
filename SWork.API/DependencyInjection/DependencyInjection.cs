namespace SWork.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSWorkDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddServiceDependencies(configuration)
                .AddRepositoryDependencies();

            return services;    
        }
    }
}
