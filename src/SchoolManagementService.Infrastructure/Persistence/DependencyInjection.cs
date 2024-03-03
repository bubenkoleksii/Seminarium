using SchoolManagementService.Core.Application.Common.DataContext;

namespace SchoolManagementService.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<QueryContext>(options =>
            options.UseNpgsql(connectionString, o => o.MigrationsHistoryTable(BaseContext.MigrationsTableName, BaseContext.Schema)));
        services.AddDbContext<CommandContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IQueryContext, QueryContext>();
        services.AddScoped<ICommandContext, CommandContext>();

        return services;
    }
}
