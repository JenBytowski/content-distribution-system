namespace DistributionSystemApi.Data;

using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data.EntitiesBuilder;
using Microsoft.EntityFrameworkCore;

public class ContentDistributionSystemContext : DbContext
{
    public ContentDistributionSystemContext(DbContextOptions<ContentDistributionSystemContext> options)
        : base(options)
    {
    }

    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    public DbSet<Recipient> Recipient { get; set; }

    public DbSet<RecipientGroup> RecipientGroup { get; set; }

    public DbSet<RecipientRecipientGroup> RecipientRecipientGroup { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NotificationTemplateEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipientEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipientGroupEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipientRecipientGroupEntityTypeConfiguration());
    }
}