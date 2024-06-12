namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(course => course.Id);

        builder.Property(course => course.Id).ValueGeneratedOnAdd();
        builder.Property(course => course.Name).HasMaxLength(250).IsRequired();
        builder.Property(course => course.Description).HasMaxLength(1024);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Course> builder)
    {
        AddCourseTeacherRelationship(builder);
        AddCourseGroupRelationship(builder);
    }

    private static void AddCourseTeacherRelationship(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(c => c.Teachers)
            .WithMany(ct => ct.Courses)
            .UsingEntity<CourseTeacherCourse>(
              j => j
                  .HasOne(ctc => ctc.Teacher)
                  .WithMany()
                  .HasForeignKey(ctc => ctc.TeacherId)
                  .OnDelete(DeleteBehavior.SetNull),
              j => j
                  .HasOne(ctc => ctc.Course)
                  .WithMany()
                  .HasForeignKey(ctc => ctc.CourseId)
                  .OnDelete(DeleteBehavior.SetNull),
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

    private static void AddCourseGroupRelationship(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(c => c.Groups)
         .WithMany(cg => cg.Courses)
         .UsingEntity<CourseGroupCourse>(
             j => j
                 .HasOne(cgc => cgc.Group)
                 .WithMany()
                 .HasForeignKey(cgc => cgc.GroupId)
                 .OnDelete(DeleteBehavior.SetNull),
             j => j
                 .HasOne(cgc => cgc.Course)
                 .WithMany()
                 .HasForeignKey(cgc => cgc.CourseId)
                 .OnDelete(DeleteBehavior.SetNull),
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
