using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace MailLibrary
{
    public class SMTPMailService : IMailValidationService, IDisposable
    {
        private const string InvalidEmaisRecipientCountExceptionMessage = "Must have at least one recipient";

        private const string InvalidEmaisFormatExceptionMessage = "Check mails format";

        private const string InvalidEmaisSendExceptionMessage = "An error occurred while sending the email: ";

        private readonly SmtpClient _smptClient;

        private readonly IMailValidationService _mailValidationService;

        public SMTPMailService(SmtpClient smtpClient, IMailValidationService mailValidationService)
        {
            _mailValidationService = mailValidationService ?? throw new ArgumentNullException(nameof(mailValidationService));

            _smptClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
        }

        public async Task SendEmailAsync(MailModel mail)
        {
            _mailValidationService.ValidateMailAndThrowError(mail);

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.Subject = mail.Subject;
                    mailMessage.Body = mail.Body;
                    mailMessage.From = new MailAddress(mail.From);
                    mail.To.ForEach(recipient => mailMessage.To.Add(new MailAddress(recipient)));
                    mail.ReplyTo.ForEach(replyToAddress => mailMessage.ReplyToList.Add(new MailAddress(replyToAddress)));
                    mail.Attachments.ForEach(attachmentPath => mailMessage.Attachments.Add(new Attachment(attachmentPath)));

                    await _smptClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(InvalidEmaisSendExceptionMessage + ex.Message);
            }
        }

        public void ValidateMailAndThrowError(MailModel mail)
        {
            if (mail.To.Count == 0)
            {
                throw new ArgumentException(InvalidEmaisRecipientCountExceptionMessage);
            }

            if(!new EmailAddressAttribute().IsValid(mail.From) && mail.To.All(address => !new EmailAddressAttribute().IsValid(address)))
            {
                throw new ArgumentException(InvalidEmaisFormatExceptionMessage);
            }
        }

        public void Dispose()
        {
            _smptClient.Dispose();
        }
    }
}
