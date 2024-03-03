namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class AlreadyExistsError : Error
{
    public override string Detail { get; set; } = "Unknown uniqueness error.";

    public override string Type => ErrorTypes.Uniqueness;
}
