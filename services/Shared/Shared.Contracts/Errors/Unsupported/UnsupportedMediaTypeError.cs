namespace Shared.Contracts.Errors.Unsupported
{
    public class UnsupportedMediaTypeError(string resourceName = "resource") : Error
    {
        public override string Detail { get; set; } = $"The {resourceName} has unsupported media type.";

        public override string Type => ErrorTypes.UnsupportedMediaType;
    }
}
