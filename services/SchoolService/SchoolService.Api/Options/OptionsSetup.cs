namespace SchoolService.Api.Options;

public static class OptionsSetup
{
    public static IServiceCollection SetupOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));
        services.Configure<S3Options>(configuration.GetSection(nameof(S3Options)));
        services.Configure<Shared.Contracts.Options.FileOptions>(configuration.GetSection("FileOptions"));

        return services;
    }
}
