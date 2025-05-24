namespace SWork.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSWorkDependencies(this IServiceCollection services)
        {
            services
                .AddServiceDependencies()
                .AddRepositoryDependencies();

            return services;    
        }
    }
}
