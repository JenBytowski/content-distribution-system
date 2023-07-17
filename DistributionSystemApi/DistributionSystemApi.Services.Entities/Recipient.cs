using System;

namespace DistributionSystemApi.Data.Entities
{
    public class Recipient
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string? TelephoneNumber { get; set; }

        public Guid? GroupId { get; set; }

        public RecipientGroup? Group { get; set; }
    }
}