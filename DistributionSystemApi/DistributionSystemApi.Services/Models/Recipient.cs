namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    using global::DistributionSystemApi.Data.Entities;

    public class GetRecipient : BaseEntity
    {
        public string Title { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public List<RecipientRecipientGroupModel> Groups { get; set; }
    }
}