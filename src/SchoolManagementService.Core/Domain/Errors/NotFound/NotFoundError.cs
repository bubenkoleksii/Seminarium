namespace SchoolManagementService.Core.Domain.Errors.NotFound;

public class NotFoundError : Error
{
    public override string Detail { get; set; } = "The object not found.";

    public override string Type => ErrorTypes.NotFound;
}
