namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class GroupNoticeConfiguration : IEntityTypeConfiguration<GroupNotice>
{
    public void Configure(EntityTypeBuilder<GroupNotice> builder)
    {
        builder.HasKey(notice => notice.Id);

        builder.Property(notice => notice.Id).ValueGeneratedOnAdd();
        builder.Property(notice => notice.Title).HasMaxLength(256).IsRequired();
        builder.Property(notice => notice.Text).HasMaxLength(2048);
        builder.Property(notice => notice.IsCrucial).IsRequired();

        builder.HasQueryFilter(notice => !notice.IsArchived);

        ConfigureRelationships(builder);

        AddFullTextIndex(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<GroupNotice> builder)
    {
        AddGroupForeignKey(builder);
        AddAuthorForeignKey(builder);
    }

    private static void AddGroupForeignKey(EntityTypeBuilder<GroupNotice> builder)
    {
        builder.HasOne(notice => notice.Group)
            .WithMany(group => group.Notices)
            .HasForeignKey(notice => notice.GroupId)
            .IsRequired();
    }

    private static void AddAuthorForeignKey(EntityTypeBuilder<GroupNotice> builder)
    {
        builder.HasOne(notice => notice.Author)
            .WithMany(author => author.Notices)
            .HasForeignKey(notice => notice.AuthorId)
            .IsRequired(required: false);
    }

    private static void AddFullTextIndex(EntityTypeBuilder<GroupNotice> builder)
    {
        builder.HasIndex(notice => new { notice.Title, notice.Text })
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("simple");
    }
}
