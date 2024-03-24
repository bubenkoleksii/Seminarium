namespace S3Service.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration appConfiguration)
    {
        ConfigureMassTransit(services, appConfiguration);
        return services;
    }

    private static void ConfigureMassTransit(IServiceCollection services, IConfiguration appConfiguration)
    {
        try
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumer<SaveFileConsumer>();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    var host = appConfiguration["RabbitMq:Host"]!;
                    configurator.Host(host, h =>
                    {
                        var username = appConfiguration["RabbitMq:Username"]!;
                        var password = appConfiguration["RabbitMq:Password"]!;

                        h.Username(username);
                        h.Password(password);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while mass transit initialization.");
            throw;
        }
    }
}
