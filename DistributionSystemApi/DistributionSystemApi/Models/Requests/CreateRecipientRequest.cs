namespace DistributionSystemApi.Requests
{
    public class CreateRecipientRequest
    {
        public string? Title { get; set; }

        public string? Email { get; set; }

        public string? TelephoneNumber { get; set; }

        public List<Guid>? Groups { get; set; }
    }
}