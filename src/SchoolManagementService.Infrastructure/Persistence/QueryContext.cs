using SchoolManagementService.Core.Application.DataContext;

namespace SchoolManagementService.Infrastructure.Persistence;

public class QueryContext : BaseContext, IQueryContext
{
    public QueryContext(DbContextOptions options) : base(options)
    {
    }
}
