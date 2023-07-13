using DistributionSystemApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DistributionSystemApi.ConfigurationExtensions
{
    public static class DatabaseConfigurationExtension
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ContentDistributionSystemContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ContentDistributionSystem")));
        }
    }
}
