using DistributionSystemApi.MailLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributionSystemApi.MailLibrary.Interfaces
{
    public interface IMailValidationService
    {
        void ValidateMailAndThrowError(MailModel mail);
    }
}