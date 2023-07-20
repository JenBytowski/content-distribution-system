namespace DistributionSystemApi.MailLibrary
{
    using System.Net;
    using System.Net.Mail;
    using DistributionSystemApi.MailLibrary.Interfaces;
    using DistributionSystemApi.MailLibrary.Models;
    using DistributionSystemApi.MailLibrary.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class MailServiceExtensions
    {
        public static IServiceCollection AddMailServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMailService, SMTPMailService>();
            services.AddScoped<IMailValidationService, MailValidationService>();
            services.AddScoped(fac =>
            {
                return CreateSmtpClient(configuration);
            });
            return services;
        }

        private static SmtpClient CreateSmtpClient(IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();

            var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
                EnableSsl = smtpSettings.EnableSsl,
            };

            return smtpClient;
        }
    }
}