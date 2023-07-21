namespace DistributionSystemApi.MailLibrary.Interfaces
{
    using DistributionSystemApi.MailLibrary.Models;

    public interface IMailValidationService
    {
        void ValidateMail(MailModel mail);
    }
}