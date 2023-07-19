using DistributionSystemApi.Data;
using DistributionSystemApi.Data.Interfaces;
using DistributionSystemApi.Interfaces;
using DistributionSystemApi.Services;

namespace DistributionSystemApi.ConfigurationExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRecipientServices(this IServiceCollection services)
        {
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IRecipientService, RecipientService>();
            services.AddScoped<IRecipientGroupService, RecipientGroupService>();

            return services;
        }
    }
}