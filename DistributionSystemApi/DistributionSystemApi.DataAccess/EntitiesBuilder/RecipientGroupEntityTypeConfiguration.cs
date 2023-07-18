﻿using DistributionSystemApi.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class RecipientGroupEntityTypeConfiguration : IEntityTypeConfiguration<RecipientGroup>
{
    public void Configure(EntityTypeBuilder<RecipientGroup> builder)
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
           .HasIndex(e => e.Title)
           .IsUnique();
    }
}