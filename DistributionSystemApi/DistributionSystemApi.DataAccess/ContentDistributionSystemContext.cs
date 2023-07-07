using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Services.Entities;

namespace DistributionSystemApi.DataAccess;

public partial class ContentDistributionSystemContext : DbContext
{
    public ContentDistributionSystemContext()
    {
    }

    public ContentDistributionSystemContext(DbContextOptions<ContentDistributionSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }

    public virtual DbSet<NotificationType> NotificationTypes { get; set; }

    public virtual DbSet<Receiver> Receivers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ContentDistributionSystem;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Group__3214EC2757DB5E90");

            entity.ToTable("Group");

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.NotificationId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NotificationID");
            entity.Property(e => e.ReceiverId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ReceiverID");

            entity.HasOne(d => d.Notification).WithMany(p => p.Groups)
                .HasForeignKey(d => d.NotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Group__Notificat__3C69FB99");

            entity.HasOne(d => d.Receiver).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Group__ReceiverI__3D5E1FD2");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Log__3214EC27BA4ED8F5");

            entity.ToTable("Log");

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LogTime).HasColumnType("datetime");
            entity.Property(e => e.NotificationId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NotificationID");

            entity.HasOne(d => d.Notification).WithMany(p => p.Logs)
                .HasForeignKey(d => d.NotificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Log__Notificatio__3B75D760");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC274904EA13");

            entity.ToTable("Notification");

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.NotificationTemplateId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NotificationTemplateID");
            entity.Property(e => e.NotificationTypeId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("NotificationTypeID");
            entity.Property(e => e.ReceiverId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ReceiverID");

            entity.HasOne(d => d.NotificationTemplate).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__398D8EEE");

            entity.HasOne(d => d.NotificationType).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Notif__38996AB5");

            entity.HasOne(d => d.Receiver).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__Recei__3A81B327");
        });

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

        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC27BDFC4376");

            entity.ToTable("NotificationType");

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Receiver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Receiver__3214EC27E6931A61");

            entity.ToTable("Receiver");

            entity.Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Login)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
