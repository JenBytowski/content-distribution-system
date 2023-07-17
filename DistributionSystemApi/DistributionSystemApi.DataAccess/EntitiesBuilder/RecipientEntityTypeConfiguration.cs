using DistributionSystemApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class RecipientEntityTypeConfiguration : IEntityTypeConfiguration<Recipient>
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .HasColumnName("Id");

        builder
            .Property(e => e.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(e => e.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(e => e.TelephoneNumber)
            .HasMaxLength(20);

        builder
            .HasOne(e => e.Group)
            .WithMany(g => g.Recipients)
            .HasForeignKey(e => e.GroupId);
    }
}
