namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists.School;

public class RegisterCodeAlreadyExists(ulong registerCode) : AlreadyExistsError
{
    public override string Detail { get; set; } = $"The school with register code '{registerCode}' already exists.";

    public override string Title => ErrorTitles.School.RegisterCodeAlreadyExists;

    public override string Type => ErrorTypes.Uniqueness;

    public override List<string> Params { get; set; } = new() { "registerCode" };
}
