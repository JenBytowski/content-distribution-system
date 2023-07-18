using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data;
using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;

namespace DistributionSystemApi.Services
{
    public class RecipientGroupService
    {
        private readonly ContentDistributionSystemContext _context;

        public RecipientGroupService(ContentDistributionSystemContext context)
        {
            _context = context;
        }

        public async Task<List<RecipientGroupResponse>> GetRecipientGroups(CancellationToken cancellationToken)
        {
            var recipientGroups = await _context.RecipientGroup.Include(g => g.Recipients).ToListAsync(cancellationToken);
            var response = recipientGroups.Select(g => new RecipientGroupResponse
            {
                Id = g.Id,
                Title = g.Title,
                Recipients = g.Recipients?.Select(r => new RecipientRecipientGroupResponse
                {
                    RecipientId = r.RecipientId,
                    GroupId = r.GroupId
                }).ToList()
            }).ToList();

            return response;
        }

        public async Task<RecipientGroupResponse> GetRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _context.RecipientGroup
                .Include(g => g.Recipients)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

            if (recipientGroup == null)
            {
                return null;
            }

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

        public async Task<RecipientGroupResponse> CreateRecipientGroup(CreateRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            var recipientGroup = new RecipientGroup
            {
                Title = request.Title
            };

            _context.RecipientGroup.Add(recipientGroup);

            if (request.RecipientIds != null && request.RecipientIds.Any())
            {
                foreach (var recipientId in request.RecipientIds)
                {
                    var recipientRecipientGroup = new RecipientRecipientGroup
                    {
                        RecipientId = recipientId,
                        GroupId = recipientGroup.Id
                    };
                    _context.RecipientRecipientGroup.Add(recipientRecipientGroup);
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
            var recipientGroup = await _context.RecipientGroup
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
                    _context.RecipientRecipientGroup.Remove(recipientRecipientGroup);
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
                _context.RecipientRecipientGroup.Add(recipientRecipientGroup);
            }

            _context.Entry(recipientGroup).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _context.RecipientGroup.FindAsync(id);

            if (recipientGroup == null)
            {
                return false;
            }

            _context.RecipientGroup.Remove(recipientGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}