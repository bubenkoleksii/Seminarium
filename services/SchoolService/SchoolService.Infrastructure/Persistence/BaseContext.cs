namespace SchoolService.Infrastructure.Persistence;

public abstract class BaseContext : DbContext
{
    public const string Schema = "public";
    public const string MigrationsTableName = "__EFMigrationsHistory";

    public DbSet<School> Schools { get; set; }

    public DbSet<JoiningRequest> JoiningRequests { get; set; }

    protected BaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());
        modelBuilder.ApplyConfiguration(new JoiningRequestConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
