namespace SchoolService.Application.School.Commands.DeleteSchool;

public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    public DeleteSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Option<Error>.None;

        _commandContext.Schools.Remove(entity);
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while archiving the school with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }
}
