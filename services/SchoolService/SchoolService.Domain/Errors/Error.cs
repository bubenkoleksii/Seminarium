namespace SchoolService.Domain.Errors;

public abstract class Error
{
    public virtual string Detail { get; set; } = "Unknown error.";

    public virtual string Title => ErrorTitles.Common.Unknown;

    public virtual string Type => ErrorTypes.Unknown;

    public virtual List<string> Params { get; set; } = new();
}
