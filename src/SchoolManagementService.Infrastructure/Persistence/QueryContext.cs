using SchoolManagementService.Core.Application.Common.DataContext;

namespace SchoolManagementService.Infrastructure.Persistence;

public sealed class QueryContext : BaseContext, IQueryContext
{
    public QueryContext(DbContextOptions<QueryContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
}
