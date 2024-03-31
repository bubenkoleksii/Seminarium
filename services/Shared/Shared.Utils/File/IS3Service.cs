namespace Shared.Utils.File;

public interface IS3Service
{
    public Task<Either<FileSuccess, Error>> UploadOne(SaveFileRequest request);

    public Either<FileSuccess, Error> GetOne(GetFileRequest request);
}
