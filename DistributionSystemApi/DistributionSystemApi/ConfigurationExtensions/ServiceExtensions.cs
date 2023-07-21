namespace DistributionSystemApi.ConfigurationExtensions
{
    using global::DistributionSystemApi.AutoMapper;
    using global::DistributionSystemApi.Data;
    using global::DistributionSystemApi.Data.Interfaces;
    using global::DistributionSystemApi.DistributionSystemApi.Services.Services;
    using global::DistributionSystemApi.Interfaces;
    using global::DistributionSystemApi.Services.Mapping;

    public static class ServiceExtensions
    {
        public static IServiceCollection AddRecipientServices(this IServiceCollection services)
        {
            services.AddScoped<IDataContext, DataContext>();
            services.AddScoped<IRecipientService, RecipientService>();
            services.AddScoped<IRecipientGroupService, RecipientGroupService>();
            services.AddAutoMapper(typeof(RecipientMappingProfile));
            services.AddAutoMapper(typeof(ServicesMappingProfile));

            return services;
        }
    }
}