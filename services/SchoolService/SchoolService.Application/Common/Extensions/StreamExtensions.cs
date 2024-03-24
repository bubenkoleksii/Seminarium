namespace SchoolService.Application.Common.Extensions;

public static class StreamExtensions
{
    public static byte[] ToArray(this Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }
}
