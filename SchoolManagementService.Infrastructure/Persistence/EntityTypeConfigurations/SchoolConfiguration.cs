﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagementService.Core.Domain.Entities;

namespace SchoolManagementService.Infrastructure.Persistence.EntityTypeConfigurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasKey(school => school.Id);
        builder.HasIndex(school => school.RegisterCode).IsUnique();
        builder.HasIndex(school => school.Img).IsUnique();
        builder.HasIndex(school => school.Email).IsUnique();

        builder.Property(school => school.Id).ValueGeneratedOnAdd();
        builder.Property(school => school.Email).HasMaxLength(50);
        builder.Property(school => school.Phone).HasMaxLength(50);
        builder.Property(school => school.SiteUrl).HasMaxLength(50);
        builder.Property(school => school.Name).HasMaxLength(250);
        builder.Property(school => school.TerritorialCommunity).HasMaxLength(250);
        builder.Property(school => school.Address).HasMaxLength(250);
        builder.Property(school => school.Img).HasMaxLength(250);

        builder.HasQueryFilter(school => !school.IsDeleted);
    }
}
