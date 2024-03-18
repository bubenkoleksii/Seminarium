namespace SchoolService.Domain.Errors;

public static class ErrorTypes
{
    public static readonly string Validation = "validation";

    public static readonly string AlreadyExists = "already_exists";

    public static readonly string NotFound = "not_found";

    public static readonly string Invalid = "invalid";

    public static readonly string UnsupportedMediaType = "unsupported_media_type";

    public static readonly string Unknown = "unknown";
}
