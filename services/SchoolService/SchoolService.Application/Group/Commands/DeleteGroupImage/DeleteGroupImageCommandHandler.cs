namespace SchoolService.Application.Group.Commands.DeleteGroupImage;

public class DeleteGroupImageCommandHandler : IRequestHandler<DeleteGroupImageCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    public DeleteGroupImageCommandHandler(ICommandContext commandContext, IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
    }

    public async Task<Option<Error>> Handle(DeleteGroupImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Groups
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.GroupId, "group");

        var deletingResult = await _filesManager.DeleteImageIfExists(entity.Img);
        if (deletingResult.IsSome)
        {
            Log.Error("An error occurred while deleting image for the group with values {@GroupId} {@FileName}.", entity.Id, entity.Img);
            return (Error)deletingResult;
        }

        entity.Img = null;
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while setting image to null for the group with id {@GroupId}.", entity.Id);

            return new InvalidDatabaseOperationError("group");
        }

        return Option<Error>.None;
    }
}
