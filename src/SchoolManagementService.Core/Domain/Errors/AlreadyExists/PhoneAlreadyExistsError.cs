namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class PhoneAlreadyExistsError : AlreadyExistsError
{
    public override string Detail { get; set; } = "The object with this phone already exists.";

    public override string Code => ErrorCodes.Common.PhoneAlreadyExists;

    public override string Type => ErrorTypes.Uniqueness;
}
