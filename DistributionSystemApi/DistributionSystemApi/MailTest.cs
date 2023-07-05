using System.Net.Mail;
using System.Net;
using MailLibrary;
using DistributionSystemApi.MailLibrary;

namespace DistributionSystemApi
{
    public class MailTest
    {
        public async void TestSend()
        {
            MailModel mail = new MailModel
            {
                Subject = "Test",
                Body = "text",
                From = "v9287808@gmail.com",
                To = new List<string> { "vadim600000@gmail.com" },
            };

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            IMailValidationService mailValidationService = new MailValidationService();
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("v9287808@gmail.com", "jqnkvtsijygpkquy");

            SMTPMailService mailService = new SMTPMailService(smtpClient, mailValidationService);

            await mailService.SendEmailAsync(mail, CancellationToken.None);
        }
    }
}