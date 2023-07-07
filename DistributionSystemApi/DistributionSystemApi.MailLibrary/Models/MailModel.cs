namespace DistributionSystemApi.MailLibrary.Models
{
    public class MailModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public EmailModel From { get; set; }

        public List<EmailModel> To { get; set; } = new List<EmailModel>();

        public List<EmailModel> ReplyTo { get; set; } = new List<EmailModel>();

        public List<string> Attachments { get; set; } = new List<string>();

        public List<AttachmentData> BinaryAttachments { get; set; } = new List<AttachmentData>();

    }
}