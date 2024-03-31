namespace Shared.Utils.File;

public class S3Service(IAmazonS3 s3Client) : IS3Service
{
    public async Task<Either<FileSuccess, Error>> UploadOne(SaveFileRequest request)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = request.Bucket,
                Key = request.Name,
                InputStream = request.Stream,
            };

            await s3Client.PutObjectAsync(putRequest);

            var preSignedUrl = GetPreSignedUrl(request.Bucket, request.Name);
            return new FileSuccess(preSignedUrl, request.Name);
        }
        catch
        {
            return new InvalidFileOperationError(request.Name, "uploading");
        }
    }

    public Either<FileSuccess, Error> GetOne(GetFileRequest request)
    {
        try
        {
            var preSignedUrl = GetPreSignedUrl(request.Bucket, request.Name);
            return new FileSuccess(preSignedUrl, request.Name);
        }
        catch
        {
            return new InvalidFileOperationError(request.Name, "retrieving");
        }
    }

    private string GetPreSignedUrl(string bucket, string name)
    {
        var preSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = name,
        };

        var preSignedUrl = s3Client.GetPreSignedURL(preSignedUrlRequest);
        return preSignedUrl;
    }
}
