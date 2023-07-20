using DistributionSystemApi.AutoMapper;
using DistributionSystemApi.Data;
using DistributionSystemApi.Data.Interfaces;
using DistributionSystemApi.DistributionSystemApi.Services.Services;
using DistributionSystemApi.Interfaces;

namespace DistributionSystemApi.ConfigurationExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRecipientServices(this IServiceCollection services)
        {
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IRecipientService, RecipientService>();
            services.AddScoped<IRecipientGroupService, RecipientGroupService>();
            services.AddAutoMapper(typeof(RecipientMappingProfile));

            return services;
        }
    }
}