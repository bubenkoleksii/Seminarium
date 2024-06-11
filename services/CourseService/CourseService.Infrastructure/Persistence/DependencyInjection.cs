namespace CourseService.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        try
        {
            services.AddDbContext<QueryContext>(options =>
                options.UseNpgsql(connectionString, o => o.MigrationsHistoryTable(BaseContext.MigrationsTableName, BaseContext.Schema)));

            services.AddDbContext<CommandContext>(options =>
                options.UseNpgsql(connectionString));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while persistence initialization.");
            throw;
        }

        services.AddScoped<IQueryContext, QueryContext>();
        services.AddScoped<ICommandContext, CommandContext>();

        return services;
    }
}
