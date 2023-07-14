using DistributionSystemApi.MailLibrary.Models;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace DistributionSystemApi.Tests
{
    public class MailLibraryTest : IClassFixture<SMTPMailServiceFixture>
    {
        private readonly SMTPMailServiceFixture _fixture;

        public MailLibraryTest(SMTPMailServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SendMailAsync_MailIsValid_SendsMail()
        {
            var mail = _fixture.GetValidMail();

            await _fixture.MailService.SendEmailAsync(mail, CancellationToken.None);

            Assert.True(_fixture.GetAmountOfSentEmails() == 1);

            _fixture.RemoveTestMails();
        }

        [Fact]
        public async Task SendMailAsync_MailIsInvalid_ThrowsException()
        {
            var mail = _fixture.GetValidMail();
            mail.From = null;

            await Assert.ThrowsAsync<ArgumentException>(
                () => _fixture.MailService.SendEmailAsync(mail, CancellationToken.None));
            Assert.True(_fixture.GetAmountOfSentEmails() == 0);
        }

        [Fact]
        public async Task SendMailsAsync_AllMailsAreValid_SendsMails()
        {
            var mails = new List<MailModel>
            {
                _fixture.GetValidMail(),
                _fixture.GetValidMail()
            };

            await _fixture.MailService.SendEmailsAsync(mails, CancellationToken.None);

            Assert.True(_fixture.GetAmountOfSentEmails() == mails.Count);

            _fixture.RemoveTestMails();
        }

        [Fact]
        public async Task SendMailsAsync_AllMailsAreInvalid_ThrowsException()
        {
            var mail = _fixture.GetValidMail();
            mail.From = null;

            var mails = new List<MailModel>
            {
                mail
            };

            await Assert.ThrowsAsync<ArgumentException>(
                () => _fixture.MailService.SendEmailsAsync(mails, CancellationToken.None));
            Assert.True(_fixture.GetAmountOfSentEmails() == 0);
        }

    }
}