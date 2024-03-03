namespace SchoolManagementService.Core.Domain.Errors.NotFound;

public class NotFoundByIdError(Guid id) : NotFoundError
{
    public override string Detail { get; set; } = $"School with id '{id}' not found.";

    public override string Type => ErrorTypes.NotFound;

    public override string Code => ErrorCodes.Common.NotFoundById;
}
