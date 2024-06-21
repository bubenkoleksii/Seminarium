namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class CourseGroupConfiguration : IEntityTypeConfiguration<CourseGroup>
{
    public void Configure(EntityTypeBuilder<CourseGroup> builder)
    {
        builder.HasKey(group => group.Id);

        builder.Property(group => group.Id).ValueGeneratedOnAdd();

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<CourseGroup> builder)
    {
        AddCourseRelationship(builder);
    }

    private static void AddCourseRelationship(EntityTypeBuilder<CourseGroup> builder)
    {
        builder.HasMany(cg => cg.Courses)
           .WithMany(c => c.Groups)
           .UsingEntity<CourseGroupCourse>(
               j => j
                   .HasOne(cgc => cgc.Course)
                   .WithMany()
                   .HasForeignKey(cgc => cgc.CourseId)
                   .OnDelete(DeleteBehavior.SetNull),
               j => j
                   .HasOne(cgc => cgc.Group)
                   .WithMany()
                   .HasForeignKey(cgc => cgc.GroupId)
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.ToTable(nameof(CourseGroupCourse));
                   j.HasKey(cgc => cgc.Id);
                   j.Property(cgc => cgc.Id).ValueGeneratedOnAdd();
                   j.Property(cgc => cgc.GroupId).IsRequired(false);
                   j.Property(cgc => cgc.CourseId).IsRequired(false);
               }
           );
    }
}
