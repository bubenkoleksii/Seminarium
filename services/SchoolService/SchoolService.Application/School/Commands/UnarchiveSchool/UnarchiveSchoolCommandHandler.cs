namespace SchoolService.Application.School.Commands.UnarchiveSchool;

public class UnarchiveSchoolCommandHandler : IRequestHandler<UnarchiveSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    public UnarchiveSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Option<Error>> Handle(UnarchiveSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.Id, "school");

        entity.IsArchived = false;
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while unarchiving the school with values {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }
}
