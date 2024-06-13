namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class CourseTeacherConfiguration : IEntityTypeConfiguration<CourseTeacher>
{
    public void Configure(EntityTypeBuilder<CourseTeacher> builder)
    {
        builder.HasKey(teacher => teacher.Id);

        builder.Property(teacher => teacher.Id).ValueGeneratedOnAdd();
        builder.Property(teacher => teacher.IsCreator).HasDefaultValue(false);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<CourseTeacher> builder)
    {
        AddCourseRelationship(builder);
        AddLessonItemsRelationship(builder);
    }

    private static void AddCourseRelationship(EntityTypeBuilder<CourseTeacher> builder)
    {
        builder.HasMany(ct => ct.Courses)
            .WithMany(c => c.Teachers)
            .UsingEntity<CourseTeacherCourse>(
                j => j
                    .HasOne(ctc => ctc.Course)
                    .WithMany()
                    .HasForeignKey(ctc => ctc.CourseId)
                    .OnDelete(DeleteBehavior.SetNull),
                j => j
                    .HasOne(ctc => ctc.Teacher)
                    .WithMany()
                    .HasForeignKey(ctc => ctc.TeacherId)
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable(nameof(CourseTeacherCourse));
                    j.HasKey(ctc => ctc.Id);
                    j.Property(ctc => ctc.Id).ValueGeneratedOnAdd();
                    j.Property(ctc => ctc.TeacherId).IsRequired(false);
                    j.Property(ctc => ctc.CourseId).IsRequired(false);
                }
            );
    }

    private static void AddLessonItemsRelationship(EntityTypeBuilder<CourseTeacher> builder)
    {
        builder.HasMany(teacher => teacher.LessonItems)
            .WithOne(item => item.Author)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
