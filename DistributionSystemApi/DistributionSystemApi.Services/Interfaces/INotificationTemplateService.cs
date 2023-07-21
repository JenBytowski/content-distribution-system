namespace DistributionSystemApi.Services.Interfaces
{
    using DistributionSystemApi.Services.Models;

    public interface INotificationTemplateService
    {
        Task<PaginationPage<GetNotificationTemplate>> GetNotificationTemplates(int page, int pageSize, CancellationToken cancellationToken);

        Task<GetNotificationTemplate> GetNotificationTemplate(Guid id, CancellationToken cancellationToken);

        Task<Guid> CreateNotificationTemplate(CreateNotificationTemplate request, CancellationToken cancellationToken);

        Task UpdateNotificationTemplate(Guid id, CreateNotificationTemplate request, CancellationToken cancellationToken);

        Task DeleteNotificationTemplate(Guid id, CancellationToken cancellationToken);
    }
}
