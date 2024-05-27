namespace SchoolService.Application.Group.Commands.SetGroupImage;

public class SetGroupImageCommandHandler : IRequestHandler<SetGroupImageCommand, Either<FileSuccess, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    public SetGroupImageCommandHandler(ICommandContext commandContext, IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
    }

    public async Task<Either<FileSuccess, Error>> Handle(SetGroupImageCommand request, CancellationToken cancellationToken)
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

        var newFileName = $"{Guid.NewGuid()}__{request.Name}";
        var uploadingResult = await _filesManager.UploadNewImage(request.Stream, newFileName, request.UrlExpirationInMin);

        if (uploadingResult.IsLeft)
        {
            entity.Img = newFileName;

            try
            {
                await _commandContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while setting image for the group with values {@GroupId} {@FileName}.", entity.Id, request.Name);

                return new InvalidDatabaseOperationError("group");
            }
        }

        return uploadingResult;
    }
}
