namespace DistributionSystemApi.Services.Models
{
    using global::DistributionSystemApi.Data.Entities;

    public class CreateRecipientGroup : BaseEntity
    {
        public string Title { get; set; }

        public List<Guid>? RecipientIds { get; set; }
    }
}
