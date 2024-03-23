namespace SchoolService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration appConfiguration)
    {
        ConfigureMediator(services);
        ConfigureMassTransit(services, appConfiguration);

        return services;
    }

    private static void ConfigureMediator(IServiceCollection services)
    {
        try
        {
            services.AddMediatR(configuration => configuration
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while mediator initialization.");
            throw;
        }
    }

    private static void ConfigureMassTransit(IServiceCollection services, IConfiguration appConfiguration)
    {
        try
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq((_, configurator) =>
                {
                    var host = new Uri(appConfiguration["RabbitMq:Host"]!);
                    configurator.Host(host, h =>
                    {
                        var username = appConfiguration["RabbitMq:Username"]!;
                        var password = appConfiguration["RabbitMq:Password"]!;

                        h.Username(username);
                        h.Password(password);
                    });
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
