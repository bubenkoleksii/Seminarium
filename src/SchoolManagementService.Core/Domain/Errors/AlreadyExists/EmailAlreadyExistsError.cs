namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class EmailAlreadyExistsError(string email) : AlreadyExistsError
{
    public override string Detail { get; set; } = $"The object with email address '{email}' already exists.";

    public override string Code => ErrorCodes.Common.EmailAlreadyExists;

    public override string Type => ErrorTypes.Uniqueness;

    public override List<string> Params { get; set; } = new() { "email" };
}
