namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists.School;

public class RegisterCodeAlreadyExists : AlreadyExistsError
{
    public override string Detail { get; set; } = "The school with this register code already exists.";

    public override string Code => ErrorCodes.School.RegisterCodeAlreadyExists;

    public override string Type => ErrorTypes.Uniqueness;
}
