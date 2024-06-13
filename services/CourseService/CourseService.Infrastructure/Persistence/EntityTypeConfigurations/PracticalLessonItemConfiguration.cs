namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class PracticalLessonItemConfiguration : IEntityTypeConfiguration<PracticalLessonItem>
{
    public void Configure(EntityTypeBuilder<PracticalLessonItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id).ValueGeneratedOnAdd();
        builder.Property(item => item.Title).HasMaxLength(256).IsRequired();
        builder.Property(item => item.Text).HasMaxLength(2048);
        builder.Property(item => item.AllowSubmitAfterDeadline).HasDefaultValue(true);

        builder.HasQueryFilter(item => !item.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<PracticalLessonItem> builder)
    {
        AddSubmitsRelationship(builder);
    }

    private static void AddSubmitsRelationship(EntityTypeBuilder<PracticalLessonItem> builder)
    {
        builder.HasMany(item => item.Submits)
            .WithOne(submit => submit.PracticalLessonItem)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
