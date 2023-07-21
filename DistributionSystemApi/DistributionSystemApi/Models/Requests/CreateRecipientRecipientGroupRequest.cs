namespace DistributionSystemApi.Requests
{
    public class CreateRecipientRecipientGroupRequest
    {
        public Guid RecipientId { get; set; }

        public Guid GroupId { get; set; }
    }
}