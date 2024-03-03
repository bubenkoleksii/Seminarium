namespace SchoolManagementService.Core.Domain.Errors;

public abstract class Error
{
    public virtual string Detail { get; set; } = "Unknown error.";

    public virtual string Code => ErrorCodes.Common.Unknown;

    public virtual string Type => ErrorTypes.Unknown;

    public virtual List<string> Params { get; set; } = new();
}
