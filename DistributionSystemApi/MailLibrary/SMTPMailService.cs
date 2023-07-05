﻿using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using DistributionSystemApi.MailLibrary;

namespace MailLibrary
{
    public class SMTPMailService : IMailService<MailModel>
    {
        private const string InvalidSMTPClientConfExceptionMessage = "Check configuration parameters";

        private const string InvalidEmailsCountExceptionMessage = "Check mails count";

        private readonly SmtpClient _smptClient;
        private readonly IMailValidationService _mailValidationService;

        public SMTPMailService(SmtpClient smtpClient, IMailValidationService mailValidationService)
        {
            _smptClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));

            _mailValidationService = mailValidationService ?? throw new ArgumentNullException(nameof(mailValidationService));
        }

        public async Task SendEmailAsync(MailModel mail, CancellationToken cancellationToken)
        {
            _mailValidationService.ValidateMailAndThrowError(mail);

            try
            {
                using (MailMessage mailMessage = new MailMessage())
                {
                    Map(mail, mailMessage);

                    await _smptClient.SendMailAsync(mailMessage, cancellationToken);
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(InvalidSMTPClientConfExceptionMessage, ex);
            }
        }

        public async Task SendEmailsAsync(IEnumerable<MailModel> mails, CancellationToken cancellationToken)
        {
            if(mails == null)
            {
                throw new ArgumentException(InvalidEmailsCountExceptionMessage);
            }

            foreach (var mail in mails)
            {
                await SendEmailAsync(mail, cancellationToken);
            }
        }

        private void Map(MailModel mail, MailMessage mailMessage)
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
        }
    }
}