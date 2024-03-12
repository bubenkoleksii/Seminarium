namespace SchoolManagementService.Infrastructure.CloudStorage;

public static class DependencyInjection
{
    public static IServiceCollection AddCloudStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var accessKey = configuration["S3:AccessKeyId"];
        var secretAccessKey = configuration["S3:SecretAccessKey"];

        var credentials = new BasicAWSCredentials(accessKey, secretAccessKey);
        var awsOptions = new AWSOptions { Credentials = credentials, Region = RegionEndpoint.EUCentral1 };
        services.AddDefaultAWSOptions(awsOptions);

        services.AddAWSService<IAmazonS3>();

        return services;
    }
}
