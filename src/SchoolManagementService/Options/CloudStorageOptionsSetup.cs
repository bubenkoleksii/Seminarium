using SchoolManagementService.Infrastructure.CloudStorage;

namespace SchoolManagementService.Options;

public class CloudStorageOptionsSetup : IConfigureOptions<CloudStorageOptions>
{
    private readonly IConfiguration _configuration;

    public CloudStorageOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CloudStorageOptions options)
    {
        _configuration.GetSection(nameof(CloudStorageOptions))
            .Bind(options);
    }
}
