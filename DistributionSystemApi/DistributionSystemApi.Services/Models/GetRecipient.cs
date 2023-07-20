using DistributionSystemApi.Data;

namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    public class GetRecipient : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public List<RecipientRecipientGroupModel> Groups { get; set; }
    }
}