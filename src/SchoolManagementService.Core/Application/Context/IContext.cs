using SchoolManagementService.Core.Domain.Entities;

namespace SchoolManagementService.Core.Application.Context;

public interface IContext
{
    DbSet<School> Schools { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
