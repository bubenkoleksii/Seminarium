namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class AlreadyExistsError(string resourceName = "resource") : Error
{
    public override string Detail { get; set; } = $"The {resourceName} already exists..";

    public override string Type => ErrorTypes.AlreadyExists;
}
