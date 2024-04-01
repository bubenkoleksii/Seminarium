namespace Shared.Utils.File;

public interface IS3Service
{
    public Either<FileSuccess, Error> GetOne(GetFileRequest request);

    public Task<Either<FileSuccess, Error>> UploadOne(SaveFileRequest request);

    public Task<Option<Error>> DeleteOne(DeleteFileRequest request);
}
