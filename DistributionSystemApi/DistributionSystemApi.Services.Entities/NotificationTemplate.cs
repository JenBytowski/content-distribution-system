namespace DistributionSystemApi.Data.Entities;

public class NotificationTemplate
{
    public Guid Id { get; set; };

    public string Description { get; set; };

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
