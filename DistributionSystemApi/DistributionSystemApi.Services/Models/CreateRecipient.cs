namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    using global::DistributionSystemApi.Data.Entities;

    public class CreateRecipient : BaseEntity
    {
        public string Title { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public List<Guid>? Groups { get; set; }
    }
}