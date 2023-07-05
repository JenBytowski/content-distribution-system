using DistributionSystemApi.MailLibrary.Interfaces;
using DistributionSystemApi.MailLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.MailLibrary.Services
{
    public class MailValidationService : IMailValidationService
    {
        private const string InvalidEmailsRecipientCountExceptionMessage = "Must have at least one recipient";
        private const string InvalidEmailsSenderExceptionMessage = "Must have sender";
        private const string InvalidEmailsFormatExceptionMessage = "Check mails format";
        private const string InvalidAttachmentsPathExceptionMessage = "Attachment file not found: ";

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
            foreach (var attachmentPath in mail.Attachments)
            {
                if (!File.Exists(attachmentPath))
                {
                    throw new FileNotFoundException(InvalidAttachmentsPathExceptionMessage + attachmentPath);
                }
            }
        }
    }
}