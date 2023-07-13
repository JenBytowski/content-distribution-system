using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data.EntitiesBuilder;

namespace DistributionSystemApi.Data;

public class ContentDistributionSystemContext : DbContext
{
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    protected void OnModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationTemplateEntityTypeConfiguration());
    }
}

