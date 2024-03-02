using SchoolManagementService.Core.Domain;
using SchoolManagementService.Infrastructure.Persistence.EntityTypeConfigurations;

namespace SchoolManagementService.Infrastructure.Persistence;

public abstract class BaseContext : DbContext
{
    public const string Schema = "public";
    public const string MigrationsTableName = "__EFMigrationsHistory";

    public DbSet<School> Schools { get; set; }

    protected BaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new SchoolConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
