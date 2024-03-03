using SchoolManagementService.Core.Application.Common.DataContext;
using SchoolManagementService.Core.Domain;

namespace SchoolManagementService.Infrastructure.Persistence;

public sealed class CommandContext : BaseContext, ICommandContext
{
    public CommandContext(DbContextOptions<CommandContext> options) : base(options)
    {
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetAuditProperties();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        SetAuditProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        SetAuditProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditProperties()
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
            if (changedEntity.Entity is Entity entity)
                switch (changedEntity.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        break;

                    case EntityState.Modified:
                        Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                        entity.LastUpdatedAt = now;
                        break;
                }
    }
}
