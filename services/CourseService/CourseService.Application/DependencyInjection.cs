using CourseService.Application.Course.Consumers;

namespace CourseService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration appConfiguration)
    {
        ConfigureMediator(services);
        ConfigureMassTransit(services, appConfiguration);
        ConfigureS3(services, appConfiguration);

        services.Configure<MailOptions>(appConfiguration.GetSection(nameof(MailOptions)));
        services.AddScoped<IMailService, MailService>();

        services.AddScoped<IFilesManager, FilesManager>();
        services.AddScoped<IAttachmentManager, AttachmentManager>();

        services.AddScoped<ISchoolProfileAccessor, SchoolProfileAccessor>();

        services.AddMemoryCache();

        return services;
    }

    private static void ConfigureMediator(IServiceCollection services)
    {
        try
        {
            services.AddMediatR(configuration => configuration
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);

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

                AddConsumers(busConfigurator);

                AddRequestClients(busConfigurator);

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(rabbitMqOptions.Host, h =>
                    {
                        h.Username(rabbitMqOptions.Username);
                        h.Password(rabbitMqOptions.Password);
                    });

                    ConfigureReceiveEndpoints(configurator, context, rabbitMqOptions.QueueName);

                    configurator.ConfigureEndpoints(context);
                });
            });
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An error occurred while mass transit initialization.");
            throw;
        }

        static void AddConsumers(IBusRegistrationConfigurator busConfigurator)
        {
            busConfigurator.AddConsumer<DeleteCoursesConsumer>();
        }

        static void AddRequestClients(IBusRegistrationConfigurator busConfigurator)
        {
            busConfigurator.AddRequestClient<string>(new Uri($"exchange:{nameof(GetActiveSchoolProfileRequest)}"));
            busConfigurator.AddRequestClient<string>(new Uri($"exchange:{nameof(DeleteCoursesRequest)}"));
            busConfigurator.AddRequestClient<string>(new Uri($"exchange:{nameof(GetStudyPeriodsRequest)}"));
            busConfigurator.AddRequestClient<string>(new Uri($"exchange:{nameof(GetGroupsRequest)}"));
            busConfigurator.AddRequestClient<string>(new Uri($"exchange:{nameof(GetSchoolProfilesRequest)}"));
        }

        static void ConfigureReceiveEndpoints(IRabbitMqBusFactoryConfigurator configurator, IBusRegistrationContext context, string queueName)
        {
            configurator.ReceiveEndpoint($"{queueName}.{nameof(DeleteCoursesRequest)}",
                            endpointConfigurator => endpointConfigurator.ConfigureConsumer<DeleteCoursesConsumer>(context));
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
