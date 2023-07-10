using System.Net.Mail;
using DistributionSystemApi.MailLibrary.Models;
using DistributionSystemApi.MailLibrary.Interfaces;

namespace DistributionSystemApi.MailLibrary.Services
{
    public class SMTPMailService : IMailService
    {
        private const string InvalidSMTPClientConfExceptionMessage = "Check configuration parameters";
        private const string InvalidEmailsCountExceptionMessage = "Check mails count";
        private const string TaskCanceledExceptionMessage = "Operation was canceled, mail was't sent";
        private readonly SmtpClient _smptClient;
        private readonly IMailValidationService _mailValidationService;

        public SMTPMailService(SmtpClient smtpClient, IMailValidationService mailValidationService)
        {
            _smptClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));

            _mailValidationService = mailValidationService ?? throw new ArgumentNullException(nameof(mailValidationService));
        }

        public async Task SendEmailAsync(MailModel mail, CancellationToken cancellationToken)
        {
            _mailValidationService.ValidateMail(mail);

            var mailMessage = CreateMailMessage(mail);

            try
            {
                await _smptClient.SendMailAsync(mailMessage, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                throw new TaskCanceledException(TaskCanceledExceptionMessage, ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(InvalidSMTPClientConfExceptionMessage, ex);
            }
            finally
            {
                mailMessage?.Dispose();
            }
        }

        public async Task SendEmailsAsync(IEnumerable<MailModel> mails, CancellationToken cancellationToken)
        {
            if (mails == null)
            {
                throw new ArgumentException(InvalidEmailsCountExceptionMessage);
            }

            foreach (var mail in mails)
            {
                await SendEmailAsync(mail, cancellationToken);
            }
        }

        private MailMessage CreateMailMessage(MailModel mail)
        {
            var mailMessage = new MailMessage();

            mailMessage.Subject = mail.Subject;
            mailMessage.Body = mail.Body;
            mailMessage.From = new MailAddress(mail.From.Address);

            foreach (var recipient in mail.To)
            {
                mailMessage.To.Add(new MailAddress(recipient.Address));
            }

            foreach (var replyToAddress in mail.ReplyTo)
            {
                mailMessage.ReplyToList.Add(new MailAddress(replyToAddress.Address));
            }

            foreach (var attachmentPath in mail.Attachments)
            {
                mailMessage.Attachments.Add(new Attachment(attachmentPath));
            }

            foreach (AttachmentData attachment in mail.BinaryAttachments)
            {
                MemoryStream stream = new MemoryStream(attachment.Data);
                Attachment binaryAttachment = new Attachment(stream, attachment.FileName);
                mailMessage.Attachments.Add(binaryAttachment);
            }

            return mailMessage;
        }
    }
}