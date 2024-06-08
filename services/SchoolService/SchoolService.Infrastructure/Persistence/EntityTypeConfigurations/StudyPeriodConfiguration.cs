namespace SchoolService.Infrastructure.Persistence.EntityTypeConfigurations;

public class StudyPeriodConfiguration : IEntityTypeConfiguration<StudyPeriod>
{
    public void Configure(EntityTypeBuilder<StudyPeriod> builder)
    {
        builder.HasKey(period => period.Id);

        builder.Property(period => period.Id).ValueGeneratedOnAdd();
        builder.Property(period => period.StartDate).IsRequired();
        builder.Property(period => period.EndDate).IsRequired();

        builder.HasQueryFilter(school => !school.IsArchived);

        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<StudyPeriod> builder)
    {
        AddSchoolForeignKey(builder);
    }

    private static void AddSchoolForeignKey(EntityTypeBuilder<StudyPeriod> builder)
    {
        builder.HasOne(group => group.School)
            .WithMany(school => school.StudyPeriods)
            .HasForeignKey(group => group.SchoolId)
            .IsRequired();
    }
}
