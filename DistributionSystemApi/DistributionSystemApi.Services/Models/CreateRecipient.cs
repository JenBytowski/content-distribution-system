using DistributionSystemApi.Data;

namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    public class CreateRecipient : BaseEntity
    {
        public string Title { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public List<Guid>? Groups { get; set; }
    }
}