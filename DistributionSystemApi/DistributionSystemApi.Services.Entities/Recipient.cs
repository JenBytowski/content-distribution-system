namespace DistributionSystemApi.Data.Entities
{
    using System.Collections.Generic;

    public class Recipient : BaseEntity
    {
        public string Title { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public List<RecipientRecipientGroup> Groups { get; set; }
    }
}