namespace DistributionSystemApi.Data.Entities
{
    using System.Collections.Generic;

    public class RecipientGroup : BaseEntity
    {
        public string Title { get; set; }

        public List<RecipientRecipientGroup> Recipients { get; set; }
    }
}