namespace DistributionSystemApi.MailLibrary.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mail;

    public class EmailAddress
    {
        public string Address { get; set; }

        public EmailAddress(string address)
        {
            if (!IsValidAddress(address))
                throw new ArgumentException("Invalid email format ", nameof(address));

            Address = address;
        }

        private bool IsValidAddress(string address)
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
