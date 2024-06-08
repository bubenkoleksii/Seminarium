namespace SchoolService.Infrastructure.Persistence;

public abstract class BaseContext : DbContext
{
    public const string Schema = "public";
    public const string MigrationsTableName = "__EFMigrationsHistory";

    public DbSet<School> Schools { get; set; }

    public DbSet<JoiningRequest> JoiningRequests { get; set; }

    public DbSet<SchoolProfile> SchoolProfiles { get; set; }

    public DbSet<Group> Groups { get; set; }

    public DbSet<StudyPeriod> StudyPeriods { get; set; }

    public DbSet<GroupNotice> GroupNotices { get; set; }

    protected BaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new JoiningRequestConfiguration());
        modelBuilder.ApplyConfiguration(new SchoolProfileConfiguration());
        modelBuilder.ApplyConfiguration(new GroupConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
