namespace SchoolService.Application.Common.Files;

public class FilesManager : IFilesManager
{
    private readonly IS3Service _s3Service;

    private readonly S3Options _s3Options;

    public FilesManager(IS3Service s3Service, IOptions<S3Options> s3Options)
    {
        _s3Service = s3Service;
        _s3Options = s3Options.Value;
    }

    public async Task<Either<FileSuccess, Error>> UploadNewImage(Stream stream, string fileName, int? urlExpirationInMin)
    {
        var newFileRequest = urlExpirationInMin != null
            ? new SaveFileRequest(stream, fileName, _s3Options.Bucket, urlExpirationInMin.Value)
            : new SaveFileRequest(stream, fileName, _s3Options.Bucket);

        return await _s3Service.UploadOne(newFileRequest);
    }

    public async Task<Option<Error>> DeleteImageIfExists(string? name)
    {
        if (name == null)
            return Option<Error>.None;

        var deletingRequest = new DeleteFileRequest(name, _s3Options.Bucket);
        var deletingResult = await _s3Service.DeleteOne(deletingRequest);

        if (deletingResult.IsSome)
            return (Error)deletingResult;

        return Option<Error>.None;
    }
}
