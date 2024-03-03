namespace SchoolManagementService.Core.Application.Common.DataContext;

public interface IContext
{
    DbSet<Domain.School> Schools { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
