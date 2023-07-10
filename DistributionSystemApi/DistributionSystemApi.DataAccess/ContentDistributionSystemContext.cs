using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Data.Entities;

namespace DistributionSystemApi.Data;

public class ContentDistributionSystemContext : DbContext
{
    public DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ContentDistributionSystem;Trusted_Connection=True;TrustServerCertificate=True");

        modelBuilder.Entity<NotificationTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC27C8A34ED2");

            entity.ToTable("NotificationTemplate");

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

