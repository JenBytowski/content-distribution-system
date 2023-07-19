using DistributionSystemApi.Models;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;

namespace DistributionSystemApi.Interfaces
{
    public interface IRecipientService
    {
        Task<PaginationPage<RecipientResponse>> GetRecipients(int page, int pageSize, CancellationToken cancellationToken);

        Task<RecipientResponse> GetRecipient(Guid id, CancellationToken cancellationToken);

        Task<RecipientResponse> CreateRecipient(CreateRecipientRequest request, CancellationToken cancellationToken);

        Task<bool> UpdateRecipient(Guid id, CreateRecipientRequest request, CancellationToken cancellationToken);

        Task<bool> DeleteRecipient(Guid id, CancellationToken cancellationToken);
    }
}
