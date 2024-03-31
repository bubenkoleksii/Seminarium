namespace Shared.Contracts.Errors.Invalid
{
    public class SizeExceedsAllowedError(int maxSizeInMb, string resourceName = "resource") : InvalidError
    {
        public override string Detail { get; set; } =
            $"The {resourceName} size is invalid and more than {maxSizeInMb} MB.";

        public override string Title => ErrorTitles.Common.InvalidSize;
    }
}
