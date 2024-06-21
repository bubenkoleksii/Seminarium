namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.Property(a => a.Url).HasMaxLength(1024).IsRequired();

        builder.HasQueryFilter(a => !a.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Attachment> builder)
    {
        AddLessonItemRelationship(builder);
        AddPracticalLessonItemSubmitRelationship(builder);
    }

    private static void AddLessonItemRelationship(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasOne(a => a.LessonItem)
            .WithMany(item => item.Attachments)
            .HasForeignKey(a => a.LessonItemId)
            .IsRequired(required: false);
    }

    private static void AddPracticalLessonItemSubmitRelationship(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasOne(a => a.PracticalLessonItemSubmit)
            .WithMany(submit => submit.Attachments)
            .HasForeignKey(a => a.PracticalLessonItemSubmitId)
            .IsRequired(required: false);
    }
}
