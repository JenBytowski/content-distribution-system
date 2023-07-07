using System.Collections.Generic;

namespace DistributionSystemApi.Services.Entities;

public partial class Receiver
{
    public string Id { get; set; } = null!;

    public string Login { get; set; } = null!;

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
