namespace CourseService.Application.Common.Files;

public interface IFilesManager
{
    public Task<Either<FileSuccess, Error>> UploadFile(Stream stream, string fileName, int? urlExpirationInMin);

    public Task<Option<Error>> DeleteFileIfExists(string? name);

    public Either<FileSuccess, Error> GetFile(string name);
}
