using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data;
using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Interfaces;
using DistributionSystemApi.Data.Interfaces;

namespace DistributionSystemApi.Services
{
    public class RecipientGroupService : IRecipientGroupService
    {
        private readonly IDataContext _context;

        public RecipientGroupService(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<RecipientGroupResponse>> GetRecipientGroups(CancellationToken cancellationToken)
        {
            var recipientGroups = await _context.Get<RecipientGroup>()
                .Include(g => g.Recipients)
                .Select(g => new RecipientGroupResponse
                {
                    Id = g.Id,
                    Title = g.Title,
                    Recipients = g.Recipients.Select(r => new RecipientRecipientGroupResponse
                    {
                        RecipientId = r.RecipientId,
                        GroupId = r.GroupId
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return recipientGroups;
        }

        public async Task<RecipientGroupResponse> GetRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _context.Get<RecipientGroup>()
                .Include(g => g.Recipients)
                .Where(g => g.Id == id)
                .Select(g => new RecipientGroupResponse
                {
                    Id = g.Id,
                    Title = g.Title,
                    Recipients = g.Recipients.Select(r => new RecipientRecipientGroupResponse
                    {
                        RecipientId = r.RecipientId,
                        GroupId = r.GroupId
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return recipientGroup;
        }

        public async Task<RecipientGroupResponse> CreateRecipientGroup(CreateRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            if (request.Title == null)
            {
                throw new ArgumentNullException("Title and Email cannot be null");
            }

            var recipientGroup = new RecipientGroup
            {
                Title = request.Title
            };

            _context.Create(recipientGroup);

            if (request.RecipientIds != null && request.RecipientIds.Any())
            {
                foreach (var recipientId in request.RecipientIds)
                {
                    var recipientRecipientGroup = new RecipientRecipientGroup
                    {
                        RecipientId = recipientId,
                        GroupId = recipientGroup.Id
                    };
                    _context.Create(recipientRecipientGroup);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var response = new RecipientGroupResponse
            {
                Id = recipientGroup.Id,
                Title = recipientGroup.Title,
                Recipients = recipientGroup.Recipients?.Select(r => new RecipientRecipientGroupResponse
                {
                    RecipientId = r.RecipientId,
                    GroupId = r.GroupId
                }).ToList()
            };

            return response;
        }

        public async Task<bool> UpdateRecipientGroup(Guid id, CreateRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            if (request.Title == null)
            {
                throw new ArgumentNullException("Title and Email cannot be null");
            }

            var recipientGroup = await _context.Get<RecipientGroup>()
                .Include(g => g.Recipients)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

            if (recipientGroup == null)
            {
                return false;
            }

            recipientGroup.Title = request.Title;

            var existingRecipientIds = recipientGroup.Recipients.Select(r => r.RecipientId).ToList();
            var recipientsToRemove = existingRecipientIds.Except(request.RecipientIds).ToList();
            foreach (var recipientId in recipientsToRemove)
            {
                var recipientRecipientGroup = recipientGroup.Recipients.FirstOrDefault(r => r.RecipientId == recipientId);
                if (recipientRecipientGroup != null)
                {
                    recipientGroup.Recipients.Remove(recipientRecipientGroup);
                    _context.Remove(recipientRecipientGroup);
                }
            }

            var recipientsToAdd = request.RecipientIds.Except(existingRecipientIds).ToList();
            foreach (var recipientId in recipientsToAdd)
            {
                var recipientRecipientGroup = new RecipientRecipientGroup
                {
                    RecipientId = recipientId,
                    GroupId = recipientGroup.Id
                };
                recipientGroup.Recipients.Add(recipientRecipientGroup);
                _context.Create(recipientRecipientGroup);
            }

            _context.Update(recipientGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _context.Get<RecipientGroup>().SingleOrDefaultAsync(r => r.Id == id);

            if (recipientGroup == null)
            {
                return false;
            }

            _context.Remove(recipientGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}