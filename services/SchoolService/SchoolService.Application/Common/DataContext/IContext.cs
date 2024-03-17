namespace SchoolService.Application.Common.DataContext;

public interface IContext
{
    DbSet<Domain.Entities.School> Schools { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
