namespace SchoolService.Api.Options.Image;

public class ImageOptionsSetup : IConfigureOptions<ImageOptions>
{
    private readonly IConfiguration _configuration;

    public ImageOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(ImageOptions options)
    {
        _configuration.GetSection(nameof(ImageOptions))
            .Bind(options);
    }
}
