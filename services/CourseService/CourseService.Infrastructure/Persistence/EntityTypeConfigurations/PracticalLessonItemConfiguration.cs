namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class PracticalLessonItemConfiguration : IEntityTypeConfiguration<PracticalLessonItem>
{
    public void Configure(EntityTypeBuilder<PracticalLessonItem> builder)
    {
        builder.Property(item => item.AllowSubmitAfterDeadline).HasDefaultValue(true);

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
