using DistributionSystemApi.MailLibrary.Models;
using DistributionSystemApi.MailLibrary.Services;
using System.Net.Mail;

namespace DistributionSystemApi.Tests
{
    public class SMTPMailServiceFixture : IDisposable
    {
        public const string From = "example@gmail.com";
        private const string TestDirectoryPath = "Test";

        public List<string> Emails = new List<string> { "mail1@gmail.com", "mail2@gmail.com" };

        public List<string> Attachments = new List<string> { "D:\\test.txt" };

        public SMTPMailService MailService { get; set; }

        public SMTPMailServiceFixture()
        {
            MailService = new SMTPMailService(SetupSMTPClient(), new MailValidationService());
        }

        public MailModel GetValidMail()
        {
            return new MailModel
            {
                From = new EmailAddress(From),
                To = Emails.Select(email => new EmailAddress(email)).ToList(),
                Subject = "Subject",
                Body = "Body",
                Attachments = Attachments,
            };
        }

        private SmtpClient SetupSMTPClient()
        {
            var directoryWithTestMailsPath = GetDirectoryWithTestMailsPath();

            if (!Directory.Exists(directoryWithTestMailsPath))
            {
                Directory.CreateDirectory(directoryWithTestMailsPath);
            }

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = directoryWithTestMailsPath
            };

            return client;
        }

        private string GetDirectoryWithTestMailsPath()
        {
            var currentDirectoryPath = Directory.GetCurrentDirectory();
            return string.Concat(
                currentDirectoryPath,
                Path.DirectorySeparatorChar,
                TestDirectoryPath);
        }

        public void RemoveTestMails()
        {
            var mailFilePaths = Directory.GetFiles(GetDirectoryWithTestMailsPath());

            foreach (var path in mailFilePaths)
            {
                File.Delete(path);
            }
        }

        public int GetAmountOfSentEmails()
        {
            var directoryWithTestMailsPath = GetDirectoryWithTestMailsPath();

            return Directory.GetFiles(directoryWithTestMailsPath).Length;
        }

        public void Dispose()
        {
            var directoryWithTestMailsPath = GetDirectoryWithTestMailsPath();

            if (!Directory.Exists(directoryWithTestMailsPath))
            {
                return;
            }

            Directory.Delete(directoryWithTestMailsPath, true);
        }
    }
}