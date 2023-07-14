using DistributionSystemApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DistributionSystemApi.ConfigurationExtensions
{
    public static class DatabaseConfigurationExtension
    {
        public static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContentDistributionSystemContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ContentDistributionSystem")));

            return services;
        }
    }
}
