namespace DistributionSystemApi.Data.EntitiesBuilder
{
    using DistributionSystemApi.Data.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class NotificationTemplateEntityTypeConfiguration : IEntityTypeConfiguration<NotificationTemplate>
    {
        public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder
                .Property(e => e.Id)
                .HasColumnName("Id");

            builder
                .Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
        }
    }
}