using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DistributionSystemApi.Data.Entities;

public class RecipientRecipientGroupEntityTypeConfiguration : IEntityTypeConfiguration<RecipientRecipientGroup>
{
    public void Configure(EntityTypeBuilder<RecipientRecipientGroup> builder)
    {
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