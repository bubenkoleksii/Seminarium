namespace SchoolManagementService.Core.Domain.Errors.NotFound;

public class NotFoundError(string resourceName = "resource") : Error
{
    public override string Detail { get; set; } = $"The {resourceName} not found.";

    public override string Type => ErrorTypes.NotFound;
}
