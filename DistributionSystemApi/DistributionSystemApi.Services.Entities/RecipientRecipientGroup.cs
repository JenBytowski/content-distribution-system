namespace DistributionSystemApi.Data.Entities
{
    using System;

    public class RecipientRecipientGroup : BaseEntity
    {
        public Guid RecipientId { get; set; }

        public Recipient Recipient { get; set; }

        public Guid? GroupId { get; set; }

        public RecipientGroup Group { get; set; }
    }
}
