namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class SchoolProfileConfiguration : IEntityTypeConfiguration<SchoolProfile>
{
    public void Configure(EntityTypeBuilder<SchoolProfile> builder)
    {
        builder.HasKey(profile => profile.Id);

        builder.Property(profile => profile.Id).ValueGeneratedOnAdd();
        builder.Property(profile => profile.Phone).HasMaxLength(50);
        builder.Property(profile => profile.Img).HasMaxLength(250);
        builder.Property(profile => profile.Details).HasMaxLength(1024);
        builder.Property(profile => profile.Data).HasMaxLength(1024);

        builder.Property(profile => profile.Phone).HasDefaultValue(null);
        builder.Property(profile => profile.Details).HasDefaultValue(null);
        builder.Property(profile => profile.Data).HasDefaultValue(null);
        builder.Property(profile => profile.Img).HasDefaultValue(null);
        builder.Property(profile => profile.SchoolId).HasDefaultValue(null);

        builder.HasQueryFilter(profile => !profile.IsArchived);

        builder.HasOne(profile => profile.School)
            .WithMany(school => school.Teachers)
            .HasForeignKey(profile => profile.SchoolId)
            .IsRequired(required: false);
    }
}
