using System;

namespace DistributionSystemApi.Services.Entities;

public partial class Log
{
    public string Id { get; set; } = null!;

    public string NotificationId { get; set; } = null!;

    public DateTime LogTime { get; set; }

    public string? Description { get; set; }

    public virtual Notification Notification { get; set; } = null!;
}
