namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasKey(school => school.Id);

        builder.Property(school => school.Id).ValueGeneratedOnAdd();
        builder.Property(school => school.RegisterCode).HasMaxLength(50);
        builder.Property(school => school.PostalCode).HasMaxLength(50);
        builder.Property(school => school.Email).HasMaxLength(50);
        builder.Property(school => school.Phone).HasMaxLength(50);
        builder.Property(school => school.SiteUrl).HasMaxLength(50);
        builder.Property(school => school.SiteUrl).HasDefaultValue(null);
        builder.Property(school => school.Name).HasMaxLength(250);
        builder.Property(school => school.ShortName).HasMaxLength(250);
        builder.Property(school => school.TerritorialCommunity).HasMaxLength(250);
        builder.Property(school => school.Address).HasMaxLength(250);
        builder.Property(school => school.Img).HasMaxLength(250);
        builder.Property(school => school.Img).HasDefaultValue(null);

        builder.HasQueryFilter(school => !school.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<School> builder)
    {
        AddJoiningRequestForeignKey(builder);
        AddTeachersRelationship(builder);
        AddGroupsRelationship(builder);
    }

    private static void AddJoiningRequestForeignKey(EntityTypeBuilder<School> builder)
    {
        builder.HasOne(school => school.JoiningRequest)
            .WithOne(joiningRequest => joiningRequest.School)
            .HasForeignKey<School>(school => school.JoiningRequestId)
            .IsRequired();
    }

    private static void AddTeachersRelationship(EntityTypeBuilder<School> builder)
    {
        builder.HasMany(school => school.Teachers)
            .WithOne(teacher => teacher.School)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void AddGroupsRelationship(EntityTypeBuilder<School> builder)
    {
        builder.HasMany(school => school.Groups)
            .WithOne(group => group.School)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
