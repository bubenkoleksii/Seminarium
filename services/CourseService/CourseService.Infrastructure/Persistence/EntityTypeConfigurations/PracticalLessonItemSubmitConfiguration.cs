namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class PracticalLessonItemSubmitConfiguration : IEntityTypeConfiguration<PracticalLessonItemSubmit>
{
    public void Configure(EntityTypeBuilder<PracticalLessonItemSubmit> builder)
    {
        builder.HasKey(submit => submit.Id);

        builder.Property(submit => submit.Id).ValueGeneratedOnAdd();
        builder.Property(item => item.Text).HasMaxLength(2048);

        builder.HasQueryFilter(submit => !submit.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<PracticalLessonItemSubmit> builder)
    {
        AddAttachmentsRelationship(builder);
        AddPracticalLessonItemRelationship(builder);
    }

    private static void AddAttachmentsRelationship(EntityTypeBuilder<PracticalLessonItemSubmit> builder)
    {
        builder.HasMany(submit => submit.Attachments)
            .WithOne(attachment => attachment.PracticalLessonItemSubmit)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void AddPracticalLessonItemRelationship(EntityTypeBuilder<PracticalLessonItemSubmit> builder)
    {
        builder.HasOne(submit => submit.PracticalLessonItem)
            .WithMany(item => item.Submits)
            .HasForeignKey(submit => submit.PracticalLessonItemId)
            .IsRequired();
    }
}
