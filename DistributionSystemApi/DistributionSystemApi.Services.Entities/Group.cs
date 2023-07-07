namespace DistributionSystemApi.Services.Entities;

public partial class Group
{
    public string Id { get; set; } = null!;

    public string ReceiverId { get; set; } = null!;

    public string NotificationId { get; set; } = null!;

    public virtual Notification Notification { get; set; } = null!;

    public virtual Receiver Receiver { get; set; } = null!;
}
