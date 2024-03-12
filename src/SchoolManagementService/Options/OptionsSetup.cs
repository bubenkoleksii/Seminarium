namespace SchoolManagementService.Options;

public static class OptionsSetup
{
    public static IServiceCollection SetupOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudStorageOptionsSetup>(configuration);

        return services;
    }
}
