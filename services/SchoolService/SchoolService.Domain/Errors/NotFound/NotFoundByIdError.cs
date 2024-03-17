namespace SchoolService.Domain.Errors.NotFound;

public class NotFoundByIdError(Guid id, string resourceName = "resource") : NotFoundError
{
    public override string Detail { get; set; } = $"The {resourceName} with id '{id}' not found.";

    public override string Type => ErrorTypes.NotFound;

    public override string Title => ErrorTitles.Common.NotFoundById;

    public override List<string> Params { get; set; } = new() { "id" };
}
