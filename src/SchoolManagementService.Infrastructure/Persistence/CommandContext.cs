using SchoolManagementService.Core.Application.DataContext;

namespace SchoolManagementService.Infrastructure.Persistence;

public sealed class CommandContext : BaseContext, ICommandContext
{
    public CommandContext(DbContextOptions<CommandContext> options) : base(options)
    {
    }
}
