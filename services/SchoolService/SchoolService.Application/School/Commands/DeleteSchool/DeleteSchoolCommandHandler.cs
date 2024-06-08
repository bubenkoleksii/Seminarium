namespace SchoolService.Application.School.Commands.DeleteSchool;

public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    public DeleteSchoolCommandHandler(
        ICommandContext commandContext,
        IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Schools
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(s => s.JoiningRequest)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken: cancellationToken);

        if (entity is null)
            return Option<Error>.None;

        if (entity.JoiningRequest is not null)
            entity.JoiningRequest.School = null;

        await _filesManager.DeleteFileIfExists(entity.Img);

        _commandContext.Schools.Remove(entity);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the school with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("school");
        }

        return Option<Error>.None;
    }
}
