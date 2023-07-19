using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data;
using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Models;
using DistributionSystemApi.Data.Interfaces;
using DistributionSystemApi.Interfaces;
using Azure;

namespace DistributionSystemApi.Services
{
    public class RecipientService : IRecipientService
    {
        private readonly IDataContext _context;

        public RecipientService(IDataContext context)
        {
            _context = context;
        }

        public async Task<PaginationPage<RecipientResponse>> GetRecipients(int page, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await _context.Get<Recipient>().CountAsync(cancellationToken);

            var recipients = await _context.Get<Recipient>()
                .Include(r => r.Groups)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RecipientResponse
                {
                    Id = r.Id,
                    Title = r.Title,
                    Email = r.Email,
                    TelephoneNumber = r.TelephoneNumber,
                    Groups = r.Groups.Select(g => new RecipientRecipientGroupResponse
                    {
                        RecipientId = g.RecipientId,
                        GroupId = g.GroupId
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            var pageResult = new PaginationPage<RecipientResponse>
            {
                Items = recipients,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return pageResult;
        }


        public async Task<RecipientResponse> GetRecipient(Guid id, CancellationToken cancellationToken)
        {
            var recipient = await _context.Get<Recipient>()
                .Include(r => r.Groups)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (recipient == null)
            {
                return null;
            }

            var response = new RecipientResponse
            {
                Id = recipient.Id,
                Title = recipient.Title,
                Email = recipient.Email,
                TelephoneNumber = recipient.TelephoneNumber,
                Groups = recipient.Groups?.Select(g => new RecipientRecipientGroupResponse
                {
                    RecipientId = g.RecipientId,
                    GroupId = g.GroupId
                }).ToList()
            };

            return response;
        }

        public async Task<RecipientResponse> CreateRecipient(CreateRecipientRequest request, CancellationToken cancellationToken)
        {
            if (request.Title == null || request.Email == null)
            {
                throw new ArgumentNullException("Title and Email cannot be null");
            }

            var recipient = new Recipient
            {
                Title = request.Title,
                Email = request.Email,
                TelephoneNumber = request.TelephoneNumber
            };

            _context.Create(recipient);

            if (request.Groups != null && request.Groups.Any(g => g != null))
            {
                recipient.Groups = request.Groups
                    .Where(g => g != null)
                    .Select(groupId => new RecipientRecipientGroup
                    {
                        RecipientId = recipient.Id,
                        GroupId = groupId
                    })
                    .ToList();
            }

            await _context.SaveChangesAsync(cancellationToken);

            var response = new RecipientResponse
            {
                Id = recipient.Id,
                Title = recipient.Title,
                Email = recipient.Email,
                TelephoneNumber = recipient.TelephoneNumber,
                Groups = recipient.Groups?.Select(g => new RecipientRecipientGroupResponse
                {
                    RecipientId = g.RecipientId,
                    GroupId = g.GroupId
                }).ToList()
            };

            return response;
        }

        public async Task<bool> UpdateRecipient(Guid id, CreateRecipientRequest request, CancellationToken cancellationToken)
        {
            var recipient = await _context.Get<Recipient>()
                .Include(r => r.Groups)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (recipient == null)
            {
                return false;
            }

            if (request.Title == null || request.Email == null)
            {
                throw new ArgumentNullException("Title and Email cannot be null");
            }

            recipient.Title = request.Title;
            recipient.Email = request.Email;
            recipient.TelephoneNumber = request.TelephoneNumber;

            var existingGroupIds = recipient.Groups.Select(g => g.GroupId).ToList();
            var groupsToRemove = existingGroupIds.Except(request.Groups).ToList();
            foreach (var groupId in groupsToRemove)
            {
                var recipientRecipientGroup = recipient.Groups.FirstOrDefault(g => g.GroupId == groupId);
                if (recipientRecipientGroup != null)
                {
                    recipient.Groups.Remove(recipientRecipientGroup);
                    _context.Remove(recipientRecipientGroup);
                }
            }

            var groupsToAdd = request.Groups.Except(existingGroupIds).ToList();
            foreach (var groupId in groupsToAdd)
            {
                var recipientRecipientGroup = new RecipientRecipientGroup
                {
                    RecipientId = recipient.Id,
                    GroupId = groupId
                };
                recipient.Groups.Add(recipientRecipientGroup);
                _context.Create(recipientRecipientGroup);
            }

            _context.Update(recipient);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteRecipient(Guid id, CancellationToken cancellationToken)
        {
            var recipient = await _context.Get<Recipient>().SingleOrDefaultAsync(r => r.Id == id);

            if (recipient == null)
            {
                return false;
            }

            var groupRecipients = await _context.Get<RecipientRecipientGroup>()
                .Where(rg => rg.RecipientId == id)
                .ToListAsync(cancellationToken);

            _context.Remove(recipient);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}