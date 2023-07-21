namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    using global::DistributionSystemApi.Data.Entities;

    public class RecipientRecipientGroupModel : BaseEntity
    {
        public Guid? RecipientId { get; set; }

        public Guid? GroupId { get; set; }
    }
}
