namespace SchoolManagementService.Infrastructure.Persistence;

public class QueryContext : BaseContext
{
    public QueryContext(DbContextOptions options) : base(options)
    {
    }
}
