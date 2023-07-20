namespace DistributionSystemApi.Interfaces
{
    using global::DistributionSystemApi.DistributionSystemApi.Services.Models;

    public interface IRecipientService
    {
        Task<PaginationPage<GetRecipient>> GetRecipients(uint page, uint pageSize, CancellationToken cancellationToken);

        Task<GetRecipient> GetRecipient(Guid id, CancellationToken cancellationToken);

        Task<Guid> CreateRecipient(CreateRecipient request, CancellationToken cancellationToken);

        Task UpdateRecipient(Guid id, CreateRecipient request, CancellationToken cancellationToken);

        Task DeleteRecipient(Guid id, CancellationToken cancellationToken);
    }
}
