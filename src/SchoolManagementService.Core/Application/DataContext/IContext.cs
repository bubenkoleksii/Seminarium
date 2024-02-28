namespace SchoolManagementService.Core.Application.DataContext;

public interface IContext
{
    DbSet<Domain.School> Schools { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
