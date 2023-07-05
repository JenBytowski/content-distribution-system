using MailLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.MailLibrary
{
    public interface IMailValidationService
    {
        void ValidateMailAndThrowError(MailModel mail);
    }
}