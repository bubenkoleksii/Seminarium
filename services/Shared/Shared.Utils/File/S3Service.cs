namespace Shared.Utils.File;

public class S3Service(IAmazonS3 s3Client) : IS3Service
{
    public Either<FileSuccess, Error> GetOne(GetFileRequest request)
    {
        try
        {
            var preSignedUrl = GetPreSignedUrl(request.Bucket, request.Name, request.UrlExpirationInMin);
            return new FileSuccess(preSignedUrl, request.Name);
        }
        catch
        {
            return new InvalidFileOperationError(request.Name, "retrieving");
        }
    }

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
        }
        catch
        {
            return new InvalidFileOperationError(request.Name, "uploading");
        }

        try
        {
            var preSignedUrl = GetPreSignedUrl(request.Bucket, request.Name, request.UrlExpirationInMin);
            return new FileSuccess(preSignedUrl, request.Name);
        }
        catch
        {
            return new InvalidFileOperationError(request.Name, "retrieving");
        }
    }

    public async Task<Option<Error>> DeleteOne(DeleteFileRequest request)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = request.Bucket,
                Key = request.Name
            };

            await s3Client.DeleteObjectAsync(deleteRequest);
        }
        catch
        {
            return new InvalidFileOperationError(request.Name, "deleting");
        }

        return Option<Error>.None;
    }

    private string GetPreSignedUrl(string bucket, string name, int urlExpirationInMin = 100)
    {
        var preSignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = name,
            Expires = DateTime.UtcNow.AddMinutes(urlExpirationInMin)
        };

        var preSignedUrl = s3Client.GetPreSignedURL(preSignedUrlRequest);
        return preSignedUrl;
    }
}
