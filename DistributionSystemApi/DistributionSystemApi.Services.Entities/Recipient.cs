using System;
using System.Collections.Generic;

namespace DistributionSystemApi.Data.Entities
{
    public class Recipient : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string? TelephoneNumber { get; set; }

        public List<RecipientRecipientGroup>? Groups { get; set; }
    }
}