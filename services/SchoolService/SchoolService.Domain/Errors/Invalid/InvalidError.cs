namespace SchoolService.Domain.Errors.Invalid;

public class InvalidError(string resourceName = "resource") : Error
{
    public override string Detail { get; set; } = $"The {resourceName} is invalid.";

    public override string Type => ErrorTypes.Invalid;
}
