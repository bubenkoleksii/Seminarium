namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(group => group.Id);

        builder.Property(group => group.Id).ValueGeneratedOnAdd();
        builder.Property(group => group.Name).HasMaxLength(250);
        builder.Property(group => group.Img).HasMaxLength(250);
        builder.Property(group => group.Img).HasDefaultValue(null);

        builder.HasQueryFilter(group => !group.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Group> builder)
    {
        AddSchoolRelationship(builder);
        AddClassTeacherRelationship(builder);
        AddStudentsRelationship(builder);
    }

    private static void AddSchoolRelationship(EntityTypeBuilder<Group> builder)
    {
        builder.HasOne(group => group.School)
            .WithMany(school => school.Groups)
            .HasForeignKey(group => group.SchoolId)
            .IsRequired();
    }

    private static void AddClassTeacherRelationship(EntityTypeBuilder<Group> builder)
    {
        builder.HasOne(group => group.ClassTeacher)
            .WithOne(profile => profile.ClassTeacherGroup)
            .HasForeignKey<Group>(group => group.ClassTeacherId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(required: false);
    }

    private static void AddStudentsRelationship(EntityTypeBuilder<Group> builder)
    {
        builder.HasMany(group => group.Students)
            .WithOne(profile => profile.Group)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
