using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;

namespace DistributionSystemApi.Interfaces
{
    public interface IRecipientGroupService
    {
        Task<List<RecipientGroupResponse>> GetRecipientGroups(CancellationToken cancellationToken);

        Task<RecipientGroupResponse> GetRecipientGroup(Guid id, CancellationToken cancellationToken);

        Task<RecipientGroupResponse> CreateRecipientGroup(CreateRecipientGroupRequest request, CancellationToken cancellationToken);

        Task<bool> UpdateRecipientGroup(Guid id, CreateRecipientGroupRequest request, CancellationToken cancellationToken);

        Task<bool> DeleteRecipientGroup(Guid id, CancellationToken cancellationToken);
    }
}
