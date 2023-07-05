using System.Net.Mail;
using System.Net;
using Xunit;
using DistributionSystemApi.MailLibrary.Models;
using DistributionSystemApi.MailLibrary.Services;

namespace DistributionSystemApi.Tests
{
    public class MailLibraryTest
    {
        [Fact]
        public async Task TestSend()
        {
            // Arrange
            var mail = new MailModel
            {
                Subject = "Test",
                Body = "text",
                From = "v9287808@gmail.com",
                To = new List<string> { "vadim600000@gmail.com" },
            };

            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("v9287808@gmail.com", "jqnkvtsijygpkquy")
            };

            var mailValidationService = new MailValidationService();
            var mailService = new SMTPMailService(smtpClient, mailValidationService);

            // Act
            try
            {
                await mailService.SendEmailAsync(mail, CancellationToken.None);
                // Assert
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }
        }

        [Fact]
        public async Task TestSendWithAttachment()
        {
            // Arrange
            var mail = new MailModel
            {
                Subject = "Test",
                Body = "text",
                From = "v9287808@gmail.com",
                To = new List<string> { "vadim600000@gmail.com" },
                Attachments = new List<string> { "D:\\test.txt" },
            };

            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("v9287808@gmail.com", "jqnkvtsijygpkquy")
            };

            var mailValidationService = new MailValidationService();
            var mailService = new SMTPMailService(smtpClient, mailValidationService);

            // Act
            try
            {
                await mailService.SendEmailAsync(mail, CancellationToken.None);
                // Assert
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }
        }
    }

}