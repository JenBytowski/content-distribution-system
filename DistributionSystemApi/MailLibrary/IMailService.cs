using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.MailLibrary
{
    public interface IMailService<MailModel>
    {
        Task SendEmailAsync(MailModel mail, CancellationToken cancellationToken);

        Task SendEmailsAsync(IEnumerable<MailModel> mails, CancellationToken cancellationToken);
    }
}