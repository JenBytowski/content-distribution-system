namespace DistributionSystemApi.ConfigurationExtensions
{
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseConfigurationExtension
    {
        public static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.ContentDistributionSystemContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ContentDistributionSystemDatabase")));

            return services;
        }
    }
}
