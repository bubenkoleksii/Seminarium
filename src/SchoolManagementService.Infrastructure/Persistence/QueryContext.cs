using SchoolManagementService.Core.Application.DataContext;

namespace SchoolManagementService.Infrastructure.Persistence;

public class QueryContext : BaseContext, IQueryContext
{
    public QueryContext(DbContextOptions<QueryContext> options) : base(options)
    {
    }
}
