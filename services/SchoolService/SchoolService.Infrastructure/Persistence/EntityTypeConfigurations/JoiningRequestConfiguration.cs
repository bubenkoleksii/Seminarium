﻿namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class JoiningRequestConfiguration : IEntityTypeConfiguration<JoiningRequest>
{
    public void Configure(EntityTypeBuilder<JoiningRequest> builder)
    {
        builder.HasKey(request => request.Id);

        builder.Property(request => request.Id).ValueGeneratedOnAdd();
        builder.Property(request => request.RegisterCode).HasMaxLength(50);
        builder.Property(request => request.PostalCode).HasMaxLength(50);
        builder.Property(request => request.RequesterEmail).HasMaxLength(50);
        builder.Property(request => request.RequesterPhone).HasMaxLength(50);
        builder.Property(request => request.RequesterFullName).HasMaxLength(250);
        builder.Property(request => request.Name).HasMaxLength(250);
        builder.Property(request => request.ShortName).HasMaxLength(250);
        builder.Property(request => request.TerritorialCommunity).HasMaxLength(250);
        builder.Property(request => request.Address).HasMaxLength(250);
        builder.Property(request => request.Status).HasDefaultValue(JoiningRequestStatus.Created);
        builder.Property(request => request.SchoolId).HasDefaultValue(null);

        builder.HasQueryFilter(request => !request.IsArchived);

        builder.HasOne(joiningRequest => joiningRequest.School)
            .WithOne(school => school.JoiningRequest)
            .HasForeignKey<JoiningRequest>(joiningRequest => joiningRequest.SchoolId)
            .IsRequired(required: false);
    }
}
