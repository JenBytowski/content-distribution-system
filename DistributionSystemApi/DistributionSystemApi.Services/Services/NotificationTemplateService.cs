namespace DistributionSystemApi.Services.Services
{
    using AutoMapper;
    using DistributionSystemApi.Data.Entities;
    using DistributionSystemApi.Data.Interfaces;
    using DistributionSystemApi.Services.Interfaces;
    using DistributionSystemApi.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class NotificationTemplateService : INotificationTemplateService
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;

        public NotificationTemplateService(IDataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<Guid> CreateNotificationTemplate(CreateNotificationTemplate request, CancellationToken cancellationToken)
        {
            if (request.Description is null)
            {
                throw new ArgumentNullException("Description cannot be null");
            }

            var notificationTemplate = new NotificationTemplate
            {
                Description = request.Description,
            };

            _dataContext.Create(notificationTemplate);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return notificationTemplate.Id;
        }

        public async Task DeleteNotificationTemplate(Guid id, CancellationToken cancellationToken)
        {
            var notificationTemplate = await _dataContext.Get<NotificationTemplate>()
                .FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);

            if (notificationTemplate is null)
            {
                throw new ArgumentNullException("Notification template is not exist");
            }

            _dataContext.Remove(notificationTemplate);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetNotificationTemplate> GetNotificationTemplate(Guid id, CancellationToken cancellationToken)
        {
            var response = await _dataContext.Get<NotificationTemplate>()
                .Where(_ => _.Id == id)
                .Select(_ => new GetNotificationTemplate
                {
                    Id = _.Id,
                    Description = _.Description,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return response;
        }

        public async Task<PaginationPage<GetNotificationTemplate>> GetNotificationTemplates(int page, int pageSize, CancellationToken cancellationToken)
        {
            var totalCount = await _dataContext.Get<NotificationTemplate>().CountAsync(cancellationToken);

            var notificationTemplates = await _dataContext.Get<NotificationTemplate>()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(_ => new GetNotificationTemplate
                {
                    Id = _.Id,
                    Description = _.Description,
                })
                .ToListAsync(cancellationToken);

            var notificationTeplatesResponse = _mapper.Map<List<GetNotificationTemplate>>(notificationTemplates);

            var pageResut = new PaginationPage<GetNotificationTemplate>
            {
                Items = notificationTemplates,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount,
            };

            return pageResut;
        }

        public async Task UpdateNotificationTemplate(Guid id, CreateNotificationTemplate request, CancellationToken cancellationToken)
        {
            var notificationTemplate = await _dataContext.Get<NotificationTemplate>()
                .FirstOrDefaultAsync(_ => _.Id == id, cancellationToken);

            if (request.Description is null)
            {
                throw new ArgumentNullException("Description cannot be null");
            }

            notificationTemplate.Description = request.Description;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
