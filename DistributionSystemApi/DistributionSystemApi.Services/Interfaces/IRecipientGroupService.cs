﻿using DistributionSystemApi.Services.Models;

namespace DistributionSystemApi.Interfaces
{
    public interface IRecipientGroupService
    {
        Task<List<GetRecipientGroup>> GetRecipientGroups(CancellationToken cancellationToken);

        Task<GetRecipientGroup> GetRecipientGroup(Guid id, CancellationToken cancellationToken);

        Task<Guid> CreateRecipientGroup(CreateRecipientGroup request, CancellationToken cancellationToken);

        Task UpdateRecipientGroup(Guid id, CreateRecipientGroup request, CancellationToken cancellationToken);

        Task DeleteRecipientGroup(Guid id, CancellationToken cancellationToken);
    }
}
