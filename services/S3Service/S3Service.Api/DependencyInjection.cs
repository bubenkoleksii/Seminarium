namespace S3Service.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        try
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<CreateFileConsumer>();

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("school", false));

                x.UsingRabbitMq((context, configuration) =>
                {
                    configuration.ConfigureEndpoints(context);
                });
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while mass transit initialization.");
            throw;
        }

        return services;
    }
}
