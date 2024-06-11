namespace CourseService.Application.Common.DataContext;

public interface IContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
