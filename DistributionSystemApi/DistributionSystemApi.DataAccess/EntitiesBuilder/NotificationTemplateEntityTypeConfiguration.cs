using DistributionSystemApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DistributionSystemApi.Data.EntitiesBuilder
{
    public class NotificationTemplateEntityTypeConfiguration : IEntityTypeConfiguration<NotificationTemplate>
    {
        public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder
                .Property(e => e.Id)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ID");

            builder
                .Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        }
    }
}
