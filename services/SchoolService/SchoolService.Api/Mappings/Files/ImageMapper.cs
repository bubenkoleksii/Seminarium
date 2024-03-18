namespace SchoolService.Api.Mappings.Files;

public static class ImageMapper
{
    public static Either<Stream, Error> GetStreamIfValid(IFormFile file, int maxSizeInMb = 10)
    {
        var stream = file.OpenReadStream();

        var isInvalidImageType = !FileTypeValidator.IsTypeRecognizable(stream) || !stream.IsImage();
        if (isInvalidImageType)
            return new UnsupportedMediaTypeError("school's image");

        var sizeInMb = GetFileSizeInMb(file.Length);
        if (sizeInMb > maxSizeInMb)
            return new SizeExceedsAllowedError(maxSizeInMb, "school's image");

        return stream;
    }

    private static double GetFileSizeInMb(long length) => length / (1024.0 * 1024);
}
