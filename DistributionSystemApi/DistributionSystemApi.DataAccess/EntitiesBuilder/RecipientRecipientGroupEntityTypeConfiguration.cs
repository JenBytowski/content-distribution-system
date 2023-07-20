namespace DistributionSystemApi.Data.EntitiesBuilder
{
    using DistributionSystemApi.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RecipientRecipientGroupEntityTypeConfiguration : IEntityTypeConfiguration<RecipientRecipientGroup>
    {
        public void Configure(EntityTypeBuilder<RecipientRecipientGroup> builder)
        {
            builder
                .Property(e => e.Id)
                .HasColumnName("Id");

            builder
                .HasKey(rrg => new { rrg.RecipientId, rrg.GroupId });

            builder
                .HasOne(rrg => rrg.Recipient)
                .WithMany(r => r.Groups)
                .HasForeignKey(rrg => rrg.RecipientId);

            builder
                .HasOne(rrg => rrg.Group)
                .WithMany(g => g.Recipients)
                .HasForeignKey(rrg => rrg.GroupId);
        }
    }
}