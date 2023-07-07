using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace DistributionSystemApi.MailLibrary.Models
{
    public class EmailModel
    {
        public string Address { get; set; }

        public EmailModel(string address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentException("Invalid email format.", nameof(address));

            Address = address;
        }

        public bool IsValidAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;
            
            if (!new EmailAddressAttribute().IsValid(address))
                return false;

            try
            {
                var mailAddress = new MailAddress(address);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
