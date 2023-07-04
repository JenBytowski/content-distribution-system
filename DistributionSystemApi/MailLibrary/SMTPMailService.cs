using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace MailLibrary
{
    public class SMTPMailService
    {
        private const string InvalidEmailsRecipientCountExceptionMessage = "Must have at least one recipient";

        private const string InvalidEmailsSenderExceptionMessage = "Must have sender";

        private const string InvalidEmailsFormatExceptionMessage = "Check mails format";

        private const string InvalidEmailsSendExceptionMessage = "An error occurred while sending the email: ";

        private readonly SmtpClient _smptClient;

        public SMTPMailService(SmtpClient smtpClient)
        {
            _smptClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
        }

        public async Task SendEmailAsync(MailModel mail, CancellationToken cancellationToken)
        {
            ValidateMailAndThrowError(mail);

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.Subject = mail.Subject;
                    mailMessage.Body = mail.Body;
                    mailMessage.From = new MailAddress(mail.From);

                    foreach (string recipient in mail.To)
                    {
                        mailMessage.To.Add(new MailAddress(recipient));
                    }

                    foreach (string replyToAddress in mail.ReplyTo)
                    {
                        mailMessage.ReplyToList.Add(new MailAddress(replyToAddress));
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

                    await _smptClient.SendMailAsync(mailMessage, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(InvalidEmailsSendExceptionMessage + ex.Message, ex);
            }
        }

        public void ValidateMailAndThrowError(MailModel mail)
        {
            if (mail.To.Count == 0)
            {
                throw new ArgumentException(InvalidEmailsRecipientCountExceptionMessage);
            }
            if (mail.From == null)
            {
                throw new ArgumentException(InvalidEmailsSenderExceptionMessage);
            }
            if (!new EmailAddressAttribute().IsValid(mail.From) && mail.To.All(address => !new EmailAddressAttribute().IsValid(address)))
            {
                throw new ArgumentException(InvalidEmailsFormatExceptionMessage);
            }
        }
    }
}