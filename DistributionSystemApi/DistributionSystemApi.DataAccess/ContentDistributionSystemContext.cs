using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.Data.EntitiesBuilder;

namespace DistributionSystemApi.Data;

public class ContentDistributionSystemContext : DbContext
{
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ContentDistributionSystem;Trusted_Connection=True;TrustServerCertificate=True");

    protected void OnModelBuilder(ModelBuilder modelBuilder)
    {
        new NotificationTemplateEntityTypeConfiguration().Configure(modelBuilder.Entity<NotificationTemplate>());
    }
}

