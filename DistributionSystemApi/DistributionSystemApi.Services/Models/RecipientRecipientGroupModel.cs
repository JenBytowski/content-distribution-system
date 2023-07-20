using DistributionSystemApi.Data;

namespace DistributionSystemApi.DistributionSystemApi.Services.Models
{
    public class RecipientRecipientGroupModel : BaseEntity
    {
        public Guid? Id { get; set; }

        public Guid? RecipientId { get; set; }

        public Guid? GroupId { get; set; }
    }
}
