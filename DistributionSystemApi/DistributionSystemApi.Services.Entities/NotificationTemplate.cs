using System.Collections.Generic;

namespace DistributionSystemApi.Services.Entities;

public partial class NotificationTemplate
{
    public string Id { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
