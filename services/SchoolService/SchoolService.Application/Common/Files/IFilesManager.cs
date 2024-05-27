namespace SchoolService.Application.Common.Files;

public interface IFilesManager
{
    public Task<Either<FileSuccess, Error>> UploadNewImage(Stream stream, string fileName, int? urlExpirationInMin);

    public Task<Option<Error>> DeleteImageIfExists(string? name);
}
