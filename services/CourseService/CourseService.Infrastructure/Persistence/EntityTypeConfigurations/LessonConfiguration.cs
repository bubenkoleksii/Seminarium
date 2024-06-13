namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id).ValueGeneratedOnAdd();
        builder.Property(l => l.Topic).IsRequired().HasMaxLength(250);
        builder.Property(l => l.Homework).HasMaxLength(1024);
    }
}
