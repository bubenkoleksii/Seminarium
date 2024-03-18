namespace SchoolService.Api.Options;

public static class OptionsSetup
{
    public static IServiceCollection SetupOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ImageOptions>(configuration.GetSection(nameof(ImageOptions)));

        return services;
    }
}
