namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class AlreadyUnarchived(Guid id, string resourceName = "resource") : Error
{
    public override string Detail { get; set; } = $"The {resourceName} with id '{id}' already archived.";

    public override string Type => ErrorTypes.AlreadyExists;

    public override string Title => ErrorTitles.Common.AlreadyUnarchived;
}
