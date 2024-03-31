using SchoolService.Application.Common.Options;

namespace SchoolService.Api.Options;

public static class OptionsSetup
{
    public static IServiceCollection SetupOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileOptions>(configuration.GetSection(nameof(FileOptions)));
        services.Configure<RabbitMqOptions>(configuration.GetSection(nameof(RabbitMqOptions)));

        return services;
    }
}
