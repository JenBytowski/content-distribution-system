using DistributionSystemApi.MailLibrary.Interfaces;
using DistributionSystemApi.MailLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DistributionSystemApi.MailLibrary.Services
{
    public class MailValidationService : IMailValidationService
    {
        private const string InvalidEmailsRecipientCountExceptionMessage = "Must have at least one recipient";
        private const string InvalidEmailsSenderExceptionMessage = "Must have sender";
        private const string InvalidAttachmentsPathExceptionMessage = "Attachment file not found: ";
        private const string InvalidLackOfDataExceptionMessage = "Data not found in file: ";

        public void ValidateMail(MailModel mail)
        {
            if (mail.To.Count == 0)
            {
                throw new ArgumentException(InvalidEmailsRecipientCountExceptionMessage);
            }
            if (mail.From == null)
            {
                throw new ArgumentException(InvalidEmailsSenderExceptionMessage);
            }

            foreach (var attachmentPath in mail.Attachments)
            {
                if (!File.Exists(attachmentPath))
                {
                    throw new FileNotFoundException(InvalidAttachmentsPathExceptionMessage + attachmentPath);
                }
            }
            foreach (var attachment in mail.BinaryAttachments)
            {
                if (!File.Exists(attachment.FileName))
                {
                    throw new FileNotFoundException(InvalidAttachmentsPathExceptionMessage + attachment.FileName);
                }
                if (attachment.Data == null)
                {
                    throw new FileNotFoundException(InvalidLackOfDataExceptionMessage + attachment.FileName);
                }
            }
        }
    }
}