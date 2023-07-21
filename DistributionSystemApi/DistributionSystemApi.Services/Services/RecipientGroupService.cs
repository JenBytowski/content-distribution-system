using DistributionSystemApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Interfaces;
using DistributionSystemApi.Data.Interfaces;
using DistributionSystemApi.Services.Models;
using DistributionSystemApi.DistributionSystemApi.Services.Models;

namespace DistributionSystemApi.DistributionSystemApi.Services.Services
{
    public class RecipientGroupService : IRecipientGroupService
    {
        private readonly IDataContext _context;

        public RecipientGroupService(IDataContext context)
        {
            _context = context;
        }

        public async Task<List<GetRecipientGroup>> GetRecipientGroups(CancellationToken cancellationToken)
        {
            var recipientGroups = await _context.Get<RecipientGroup>()
                .Include(g => g.Recipients)
                .Select(g => new GetRecipientGroup
                {
                    Id = g.Id,
                    Title = g.Title,
                    Recipients = g.Recipients.Select(r => new RecipientRecipientGroupModel
                    {
                        RecipientId = r.RecipientId,
                        GroupId = r.GroupId
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return recipientGroups;
        }

        public async Task<GetRecipientGroup> GetRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _context.Get<RecipientGroup>()
                .Include(g => g.Recipients)
                .Where(g => g.Id == id)
                .Select(g => new GetRecipientGroup
                {
                    Id = g.Id,
                    Title = g.Title,
                    Recipients = g.Recipients.Select(r => new RecipientRecipientGroupModel
                    {
                        RecipientId = r.RecipientId,
                        GroupId = r.GroupId
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return recipientGroup;
        }

        public async Task<Guid> CreateRecipientGroup(CreateRecipientGroup request, CancellationToken cancellationToken)
        {
            if (request.Title == null)
            {
                throw new ArgumentNullException("Title and Email cannot be null");
            }

            if (_context.Get<RecipientGroup>().Any(r => r.Title == request.Title))
            {
                throw new ArgumentException("Title must be unique");
            }

            if (request.RecipientIds != null && request.RecipientIds.Any())
            {
                var existingRecipients = _context.Get<Recipient>().Select(g => g.Id).ToList();
                var validRecipients = request.RecipientIds.Where(groupId => existingRecipients.Contains(groupId));
                var invalidRecipients = request.RecipientIds.Except(validRecipients).ToList();

                if (invalidRecipients.Any())
                {
                    throw new ArgumentException("One or more selected groups do not exist");
                }
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

            return recipientGroup.Id;
        }

        public async Task UpdateRecipientGroup(Guid id, CreateRecipientGroup request, CancellationToken cancellationToken)
        {
            if (request.Title == null)
            {
                throw new ArgumentNullException("Title cannot be null");
            }

            if (_context.Get<RecipientGroup>().Any(r => r.Title == request.Title && r.Id != id))
            {
                throw new ArgumentException("Title must be unique");
            }

            if (request.RecipientIds != null && request.RecipientIds.Any())
            {
                var existingRecipients = _context.Get<Recipient>().Select(g => g.Id).ToList();
                var validRecipients = request.RecipientIds.Where(groupId => existingRecipients.Contains(groupId));
                var invalidRecipients = request.RecipientIds.Except(validRecipients).ToList();

                if (invalidRecipients.Any())
                {
                    throw new ArgumentException("One or more selected groups do not exist");
                }
            }

            var recipientGroup = await _context.Get<RecipientGroup>()
                .Include(g => g.Recipients)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

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

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRecipientGroup(Guid id, CancellationToken cancellationToken)
        {
            var recipientGroup = await _context.Get<RecipientGroup>().FirstOrDefaultAsync(r => r.Id == id);

            if (recipientGroup != null)
            {
                var groupRecipients = await _context.Get<RecipientRecipientGroup>()
                    .Where(rrg => rrg.GroupId == id)
                    .ToListAsync(cancellationToken);

                foreach (var groupRecipient in groupRecipients)
                {
                    _context.Remove(groupRecipient);
                }

                _context.Remove(recipientGroup);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private Task<bool> AllRecipientsExistAsync(IEnumerable<Guid> recipientIds)
        {
            return _context.Get<Recipient>().AllAsync(r => recipientIds.Contains(r.Id));
        }
    }
}