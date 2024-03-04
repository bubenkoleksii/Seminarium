namespace SchoolManagementService.Core.Domain.Errors.AlreadyExists;

public class PhoneAlreadyExistsError(string phone, string resourceName = "resource") : AlreadyExistsError
{
    public override string Detail { get; set; } = $"The {resourceName} with phone '{phone}' already exists.";

    public override string Title => ErrorTitles.Common.PhoneAlreadyExists;

    public override string Type => ErrorTypes.Uniqueness;

    public override List<string> Params { get; set; } = new() { "phone" };
}
