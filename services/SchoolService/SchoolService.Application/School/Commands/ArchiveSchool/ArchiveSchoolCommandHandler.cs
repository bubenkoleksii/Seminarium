namespace SchoolService.Application.School.Commands.ArchiveSchool;

public class ArchiveSchoolCommandHandler : IRequestHandler<ArchiveSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    public ArchiveSchoolCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Option<Error>> Handle(ArchiveSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools.FindAsync(request.Id);

        if (entity is null)
            return Option<Error>.None;

        entity.IsArchived = true;
        entity.LastArchivedAt = DateTime.UtcNow;

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }
}
