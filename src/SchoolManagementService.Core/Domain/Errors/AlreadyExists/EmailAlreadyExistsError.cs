namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class EmailAlreadyExistsError : AlreadyExistsError
{
    public override string Detail { get; set; } = "The object with this email address already exists.";

    public override string Code => ErrorCodes.Common.EmailAlreadyExists;

    public override string Type => ErrorTypes.Uniqueness;
}
