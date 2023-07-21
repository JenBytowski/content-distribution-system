namespace DistributionSystemApi.Responses
{
    public class RecipientGroupResponse
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public List<RecipientRecipientGroupResponse>? Recipients { get; set; }
    }
}