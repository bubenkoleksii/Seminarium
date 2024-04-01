namespace Shared.Contracts.Options;

public class S3Options
{
    public string Bucket { get; init; } = string.Empty;

    public string AccessKeyId { get; init; } = string.Empty;

    public string SecretAccessKey { get; init; } = string.Empty;
}
