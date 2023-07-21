namespace DistributionSystemApi.Services.Models
{
    using global::DistributionSystemApi.Data.Entities;
    using global::DistributionSystemApi.DistributionSystemApi.Services.Models;

    public class GetRecipientGroup : BaseEntity
    {
        public string Title { get; set; }

        public List<RecipientRecipientGroupModel>? Recipients { get; set; }
    }
}
