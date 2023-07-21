namespace DistributionSystemApi.DistributionSystemApi.Services.Services
{
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;
    using global::DistributionSystemApi.Interfaces;
    using global::DistributionSystemApi.Data.Interfaces;
    using global::DistributionSystemApi.DistributionSystemApi.Services.Models;
    using global::DistributionSystemApi.Data.Entities;

    public class RecipientService : IRecipientService
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public RecipientService(IDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationPage<GetRecipient>> GetRecipients(uint page, uint pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await _context.Get<Recipient>().CountAsync(cancellationToken);

            var recipients = await _context.Get<Recipient>()
                .Include(r => r.Groups)
                .Skip((int)((page - 1) * pageSize))
                .Take((int)pageSize)
                .Select(r => new GetRecipient
                {
                    Id = r.Id,
                    Title = r.Title,
                    Email = r.Email,
                    TelephoneNumber = r.TelephoneNumber,
                    Groups = r.Groups.Select(g => new RecipientRecipientGroupModel
                    {
                        RecipientId = g.RecipientId,
                        GroupId = g.GroupId
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            var recipientsResponse = _mapper.Map<List<GetRecipient>>(recipients);

            var pageResult = new PaginationPage<GetRecipient>
            {
                Items = recipients,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = (uint)totalCount
            };

            return pageResult;
        }


        public async Task<GetRecipient> GetRecipient(Guid id, CancellationToken cancellationToken)
        {
            var response = await _context.Get<Recipient>()
                .Include(r => r.Groups)
                .Where(r => r.Id == id)
                .Select(r => new GetRecipient
                {
                    Id = r.Id,
                    Title = r.Title,
                    Email = r.Email,
                    TelephoneNumber = r.TelephoneNumber,
                    Groups = r.Groups.Select(g => new RecipientRecipientGroupModel
                    {
                        RecipientId = g.RecipientId,
                        GroupId = g.GroupId
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }

        public async Task<Guid> CreateRecipient(CreateRecipient request, CancellationToken cancellationToken)
        {
            if (request.Title == null)
            {
                throw new ArgumentNullException("Title cannot be null");
            }

            if (request.Email == null)
            {
                throw new ArgumentNullException("Email cannot be null");
            }

            if (_context.Get<Recipient>().Any(r => r.Email == request.Email))
            {
                throw new ArgumentException("Email must be unique");
            }

            if (_context.Get<Recipient>().Any(r => r.TelephoneNumber == request.TelephoneNumber && request.TelephoneNumber != null && r.TelephoneNumber != null))
            {
                throw new ArgumentException("Telephone number must be unique");
            }

            if (request.Groups != null && request.Groups.Any())
            {
                var existingGroupIds = _context.Get<RecipientGroup>().Select(g => g.Id).ToList();
                var validGroupIds = request.Groups.Where(groupId => existingGroupIds.Contains(groupId));
                var invalidGroupIds = request.Groups.Except(validGroupIds).ToList();

                if (invalidGroupIds.Any())
                {
                    throw new ArgumentException("One or more selected groups do not exist");
                }
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

            return recipient.Id;
        }

        public async Task UpdateRecipient(Guid id, CreateRecipient request, CancellationToken cancellationToken)
        {
            var recipient = await _context.Get<Recipient>()
                .Include(r => r.Groups)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (request.Title == null)
            {
                throw new ArgumentNullException("Title cannot be null");
            }

            if (request.Email == null)
            {
                throw new ArgumentNullException("Email cannot be null");
            }

            if (_context.Get<Recipient>().Any(r => r.Email == request.Email && r.Id != id))
            {
                throw new ArgumentException("Email must be unique");
            }

            if (_context.Get<Recipient>().Any(r => r.TelephoneNumber == request.TelephoneNumber && request.TelephoneNumber != null && r.TelephoneNumber != null))
            {
                throw new ArgumentException("Telephone number must be unique");
            }

            if (request.Groups != null && request.Groups.Any())
            {
                var existingGroupIds = _context.Get<RecipientGroup>().Select(g => g.Id).ToList();
                var validGroupIds = request.Groups.Where(groupId => existingGroupIds.Contains(groupId));
                var invalidGroupIds = request.Groups.Except(validGroupIds).ToList();

                if (invalidGroupIds.Any())
                {
                    throw new ArgumentException("One or more selected groups do not exist");
                }
            }

            recipient.Title = request.Title;
            recipient.Email = request.Email;
            recipient.TelephoneNumber = request.TelephoneNumber;

            if (request.Groups != null)
            {
                var existingGroupIds = recipient.Groups.Select(g => g.GroupId).ToList();
                var requestGroupIds = new HashSet<Guid>(request.Groups);

                for (int i = recipient.Groups.Count - 1; i >= 0; i--)
                {
                    var groupId = recipient.Groups[i].GroupId;
                    if (!requestGroupIds.Contains((Guid)groupId))
                    {
                        var recipientRecipientGroup = recipient.Groups[i];
                        recipient.Groups.RemoveAt(i);
                        _context.Remove(recipientRecipientGroup);
                    }
                }

                foreach (var groupId in requestGroupIds)
                {
                    if (!existingGroupIds.Contains(groupId))
                    {
                        var recipientRecipientGroup = new RecipientRecipientGroup
                        {
                            RecipientId = recipient.Id,
                            GroupId = groupId
                        };
                        recipient.Groups.Add(recipientRecipientGroup);
                        _context.Create(recipientRecipientGroup);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteRecipient(Guid id, CancellationToken cancellationToken)
        {
            var recipient = await _context.Get<Recipient>().FirstOrDefaultAsync(r => r.Id == id);

            if (recipient != null)
            {
                var groupRecipients = await _context.Get<RecipientRecipientGroup>()
                    .Where(rg => rg.RecipientId == id)
                    .ToListAsync(cancellationToken);

                foreach (var groupRecipient in groupRecipients)
                {
                    _context.Remove(groupRecipient);
                }

                _context.Remove(recipient);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}