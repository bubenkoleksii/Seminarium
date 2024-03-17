namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasKey(school => school.Id);

        builder.Property(school => school.Id).ValueGeneratedOnAdd();
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
    }
}
