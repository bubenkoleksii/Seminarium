namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).ValueGeneratedOnAdd();
        builder.Property(l => l.Topic).IsRequired().HasMaxLength(250);
        builder.Property(l => l.Homework).HasMaxLength(1024);

        builder.HasQueryFilter(l => !l.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Lesson> builder)
    {
        AddCourseRelationship(builder);
    }

    private static void AddCourseRelationship(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasOne(lesson => lesson.Course)
            .WithMany(course => course.Lessons)
            .HasForeignKey(lesson => lesson.CourseId)
            .IsRequired();
    }
}
