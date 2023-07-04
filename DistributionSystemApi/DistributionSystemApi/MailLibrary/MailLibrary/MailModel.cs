namespace MailLibrary
{
    public class MailModel
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public List<string> To { get; set; } = new List<string>();

        public List<string> ReplyTo { get; set; } = new List<string>();

        public List<string> Attachments { get; set; } = new List<string>();

    }
}