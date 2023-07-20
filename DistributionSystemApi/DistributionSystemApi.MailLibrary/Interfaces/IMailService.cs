namespace DistributionSystemApi.MailLibrary.Interfaces
{
    using DistributionSystemApi.MailLibrary.Models;

    public interface IMailService
    {
        Task SendEmailAsync(MailModel mail, CancellationToken cancellationToken);

        Task SendEmailsAsync(IEnumerable<MailModel> mails, CancellationToken cancellationToken);
    }
}