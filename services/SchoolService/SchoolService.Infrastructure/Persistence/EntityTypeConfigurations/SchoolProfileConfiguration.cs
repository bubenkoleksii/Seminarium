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
        builder.Property(profile => profile.ClassTeacherGroupId).HasDefaultValue(null);

        builder.HasQueryFilter(profile => !profile.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<SchoolProfile> builder)
    {
        AddSchoolRelationship(builder);
        AddGroupRelationships(builder);
    }

    private static void AddSchoolRelationship(EntityTypeBuilder<SchoolProfile> builder)
    {
        builder.HasOne(profile => profile.School)
            .WithMany(school => school.Teachers)
            .HasForeignKey(profile => profile.SchoolId)
            .IsRequired(required: false);
    }

    private static void AddGroupRelationships(EntityTypeBuilder<SchoolProfile> builder)
    {
        builder.HasOne(profile => profile.Group)
            .WithMany(group => group.Students)
            .HasForeignKey(profile => profile.GroupId)
            .IsRequired(required: false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(profile => profile.ClassTeacherGroup)
            .WithOne(group => group.ClassTeacher)
            .HasForeignKey<SchoolProfile>(profile => profile.ClassTeacherGroupId)
            .IsRequired(required: false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
