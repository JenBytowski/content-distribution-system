namespace DistributionSystemApi.Requests
{
    public class CreateRecipientGroupRequest
    {
        public string? Title { get; set; }

        public List<Guid>? RecipientIds { get; set; }
    }
}