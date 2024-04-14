namespace SchoolService.Application.Common.DataContext;

public interface IContext
{
    DbSet<Domain.Entities.School> Schools { get; set; }

    DbSet<Domain.Entities.JoiningRequest> JoiningRequests { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
