namespace SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfileImage;

public class DeleteSchoolProfileImageCommandHandler : IRequestHandler<DeleteSchoolProfileImageCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    public DeleteSchoolProfileImageCommandHandler(ICommandContext commandContext, IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolProfileImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.SchoolProfiles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.SchoolProfileId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.SchoolProfileId, "school_profile");

        var deletingResult = await _filesManager.DeleteFileIfExists(entity.Img);
        if (deletingResult.IsSome)
        {
            Log.Error("An error occurred while deleting the profile image for the school profile with values {@SchoolProfileId} {@FileName}.", entity.Id, entity.Img);

            return (Error)deletingResult;
        }

        entity.Img = null;
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while setting profile image to null for the school profile with id {@SchoolProfileId}.", entity.Id);

            return new InvalidDatabaseOperationError("school_profile");
        }

        return Option<Error>.None;
    }
}

