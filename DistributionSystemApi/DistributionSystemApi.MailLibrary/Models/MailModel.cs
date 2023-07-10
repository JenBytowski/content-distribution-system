namespace DistributionSystemApi.MailLibrary.Models
{
    public class MailModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailAddress From { get; set; }

        public List<EmailAddress> To { get; set; } = new List<EmailAddress>();

        public List<EmailAddress> ReplyTo { get; set; } = new List<EmailAddress>();

        public List<string> Attachments { get; set; } = new List<string>();

        public List<AttachmentData> BinaryAttachments { get; set; } = new List<AttachmentData>();

    }
}