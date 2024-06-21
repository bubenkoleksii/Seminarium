namespace CourseService.Infrastructure.Persistence.EntityTypeConfigurations;

public class LessonItemConfiguration : IEntityTypeConfiguration<LessonItem>
{
    public void Configure(EntityTypeBuilder<LessonItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Id).ValueGeneratedOnAdd();
        builder.Property(item => item.Title).HasMaxLength(256).IsRequired();
        builder.Property(item => item.Text).HasMaxLength(2048);

        builder.HasQueryFilter(item => !item.IsArchived);

        AddDiscriminator(builder);
        ConfigureRelationships(builder);
    }

    public static void ConfigureRelationships(EntityTypeBuilder<LessonItem> builder)
    {
        AddAttachmentsRelationship(builder);
        AddAuthorRelationship(builder);
        AddLessonRelationship(builder);
    }

    private static void AddDiscriminator(EntityTypeBuilder<LessonItem> builder)
    {
        builder.HasDiscriminator<LessonItemType>(nameof(LessonItemType))
            .HasValue<LessonItem>(LessonItemType.Base)
            .HasValue<TheoryLessonItem>(LessonItemType.Theory)
            .HasValue<PracticalLessonItem>(LessonItemType.Practical);
    }

    private static void AddAttachmentsRelationship(EntityTypeBuilder<LessonItem> builder)
    {
        builder.HasMany(item => item.Attachments)
            .WithOne(attachment => attachment.LessonItem)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void AddAuthorRelationship(EntityTypeBuilder<LessonItem> builder)
    {
        builder.HasOne(item => item.Author)
            .WithMany(author => author.LessonItems)
            .HasForeignKey(item => item.AuthorId)
            .IsRequired();
    }

    private static void AddLessonRelationship(EntityTypeBuilder<LessonItem> builder)
    {
        builder.HasOne(item => item.Lesson)
            .WithMany(lesson => lesson.LessonItems)
            .HasForeignKey(item => item.LessonId)
            .IsRequired();
    }
}
