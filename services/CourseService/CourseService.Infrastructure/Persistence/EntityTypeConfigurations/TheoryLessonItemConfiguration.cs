namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class TheoryLessonItemConfiguration : IEntityTypeConfiguration<TheoryLessonItem>
{
    public void Configure(EntityTypeBuilder<TheoryLessonItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id).ValueGeneratedOnAdd();
        builder.Property(item => item.Title).HasMaxLength(256).IsRequired();
        builder.Property(item => item.Text).HasMaxLength(2048);

        builder.HasQueryFilter(item => !item.IsArchived);
    }
}
