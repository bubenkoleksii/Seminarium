namespace Shared.Utils.File;

public static class FileMapper
{
    public static Either<Stream, Error> GetStreamIfValid(IFormFile file, bool isImage, int maxSizeInMb = 10)
    {
        var stream = file.OpenReadStream();

        var isInvalidImageType = !FileTypeValidator.IsTypeRecognizable(stream);
        if (isImage)
            isInvalidImageType = isInvalidImageType && !stream.IsImage();

        if (isInvalidImageType)
            return new UnsupportedMediaTypeError();

        var sizeInMb = GetFileSizeInMb(file.Length);
        if (sizeInMb > maxSizeInMb)
            return new SizeExceedsAllowedError(maxSizeInMb);

        return stream;
    }

    private static double GetFileSizeInMb(long length) => length / (1024.0 * 1024);
}
