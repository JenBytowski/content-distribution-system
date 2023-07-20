namespace DistributionSystemApi.Responses
{
    public class RecipientResponse
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Email { get; set; }

        public string? TelephoneNumber { get; set; }

        public List<RecipientRecipientGroupResponse>? Groups { get; set; }
    }
}