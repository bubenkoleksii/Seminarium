namespace CourseService.Infrastructure.Persistence;

public abstract class BaseContext : DbContext
{
    public const string Schema = "public";
    public const string MigrationsTableName = "__EFMigrationsHistory";

    protected BaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        base.OnModelCreating(modelBuilder);
    }
}
