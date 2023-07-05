using DistributionSystemApi.MailLibrary.Interfaces;
using DistributionSystemApi.MailLibrary.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DistributionSystemApi.MailLibrary
{
    public static class MailServiceExtensions
    {
        public static IServiceCollection AddMailServices(this IServiceCollection services)
        {
            services.AddScoped<IMailService, SMTPMailService>();
            services.AddScoped<IMailValidationService, MailValidationService>();
            return services;
        }
    }
}
