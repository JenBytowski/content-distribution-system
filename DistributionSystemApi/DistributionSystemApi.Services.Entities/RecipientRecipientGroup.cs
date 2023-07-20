using System;

namespace DistributionSystemApi.Data.Entities
{
    public class RecipientRecipientGroup : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid RecipientId { get; set; }

        public Recipient Recipient { get; set; }

        public Guid? GroupId { get; set; }

        public RecipientGroup Group { get; set; }
    }
}
