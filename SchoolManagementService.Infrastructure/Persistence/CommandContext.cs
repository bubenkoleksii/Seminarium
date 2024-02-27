using Microsoft.EntityFrameworkCore;

namespace SchoolManagementService.Infrastructure.Persistence;

public class CommandContext : BaseContext
{
    public CommandContext(DbContextOptions options) : base(options)
    {
    }
}
