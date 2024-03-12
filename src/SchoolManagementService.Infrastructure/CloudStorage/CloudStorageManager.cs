using SchoolManagementService.Core.Application.Common.CloudStorage;
using SchoolManagementService.Core.Application.Common.CloudStorage.Models;

namespace SchoolManagementService.Infrastructure.CloudStorage;

public class CloudStorageManager : ICloudStorageManager
{
    private readonly IAmazonS3 _s3Client;

    private readonly IOptions<CloudStorageOptions> _options;

    public CloudStorageManager(IOptions<CloudStorageOptions> options, IAmazonS3 s3Client)
    {
        _options = options;
        _s3Client = s3Client;
    }

    public async Task Upload(CloudStorageItem item)
    {
        var bucketName = _options.Value.BucketName;
        var fileKey = Path.Combine(item.Folder, item.FileName);

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = fileKey,
            InputStream = item.Stream
        };

        await _s3Client.PutObjectAsync(putRequest);
    }
}
