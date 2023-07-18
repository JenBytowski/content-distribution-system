using DistributionSystemApi.Services;

namespace DistributionSystemApi.ConfigurationExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRecipientServices(this IServiceCollection services)
        {
            services.AddScoped<RecipientGroupService>();
            services.AddScoped<RecipientService>();

            return services;
        }
    }
}