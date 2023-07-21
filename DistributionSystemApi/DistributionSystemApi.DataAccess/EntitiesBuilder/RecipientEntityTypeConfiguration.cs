namespace DistributionSystemApi.Data.EntitiesBuilder
{
    using DistributionSystemApi.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .HasMaxLength(256)
                .IsRequired();

            builder
                .Property(e => e.TelephoneNumber)
                .HasMaxLength(15);

            builder
                .HasIndex(e => e.TelephoneNumber)
                .IsUnique();

            builder
                .HasIndex(e => e.Email)
                .IsUnique();
        }
    }
}