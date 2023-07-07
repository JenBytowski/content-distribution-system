using System.Collections.Generic;

namespace DistributionSystemApi.Services.Entities;

public partial class Notification
{
    public string Id { get; set; } = null!;

    public string NotificationTypeId { get; set; } = null!;

    public string NotificationTemplateId { get; set; } = null!;

    public string ReceiverId { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual NotificationTemplate NotificationTemplate { get; set; } = null!;

    public virtual NotificationType NotificationType { get; set; } = null!;

    public virtual Receiver Receiver { get; set; } = null!;
}
