using DistributionSystemApi.MailLibrary.Models;

namespace DistributionSystemApi.MailLibrary.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailModel mail, CancellationToken cancellationToken);

        Task SendEmailsAsync(IEnumerable<MailModel> mails, CancellationToken cancellationToken);
    }
}