namespace SchoolService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration appConfiguration)
    {
        ConfigureMediator(services);
        ConfigureMassTransit(services, appConfiguration);
        ConfigureS3(services, appConfiguration);

        services.Configure<MailOptions>(appConfiguration.GetSection(nameof(MailOptions)));
        services.AddScoped<IMailService, MailService>();

        services.AddSingleton<IAesCipher, AesCipher>();

        services.AddScoped<IInvitationManager, InvitationManager>();

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
                var rabbitMqOptions = appConfiguration.GetSection(nameof(RabbitMqOptions)).Get<RabbitMqOptions>()!;

                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(rabbitMqOptions.Host, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);
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

    private static void ConfigureS3(IServiceCollection services, IConfiguration appConfiguration)
    {
        try
        {
            var s3Options = appConfiguration.GetSection(nameof(S3Options)).Get<S3Options>()!;

            var credentials = new BasicAWSCredentials(s3Options.AccessKeyId, s3Options.SecretAccessKey);
            var awsOptions = new AWSOptions { Credentials = credentials, Region = RegionEndpoint.EUCentral1 };
            services.AddDefaultAWSOptions(awsOptions);

            services.AddAWSService<IAmazonS3>();

            services.AddScoped<IS3Service, S3Service>();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while s3 initialization.");
            throw;
        }
    }
}
