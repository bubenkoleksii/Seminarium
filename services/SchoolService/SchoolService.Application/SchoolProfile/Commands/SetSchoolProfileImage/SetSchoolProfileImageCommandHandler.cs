namespace SchoolService.Application.SchoolProfile.Commands.SetSchoolProfileImage;

public class SetSchoolProfileImageCommandHandler : IRequestHandler<SetSchoolProfileImageCommand, Either<FileSuccess, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    private readonly ISchoolProfileManager _schoolProfileManager;

    public SetSchoolProfileImageCommandHandler(ICommandContext commandContext, IFilesManager filesManager, ISchoolProfileManager schoolProfileManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<Either<FileSuccess, Error>> Handle(SetSchoolProfileImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.SchoolProfiles
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.SchoolProfileId, cancellationToken: cancellationToken);

        if (entity is null || entity.UserId != request.UserId)
            return new NotFoundByIdError(request.SchoolProfileId, "school_profile");

        var deletingResult = await _filesManager.DeleteFileIfExists(entity.Img);
        if (deletingResult.IsSome)
        {
            Log.Error("An error occurred while deleting the profile image for the school with values {@SchoolProfileId} {@FileName}.", entity.Id, entity.Img);
            return (Error)deletingResult;
        }

        var newFileName = $"{Guid.NewGuid()}_school_profile_{request.Name}";
        var uploadingResult = await _filesManager.UploadFile(request.Stream, newFileName, request.UrlExpirationInMin);

        if (uploadingResult.IsLeft)
        {
            entity.Img = newFileName;

            try
            {
                await _commandContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while setting the profile image for the school with values {@SchoolProfileId} {@FileName}.", entity.Id, request.Name);

                return new InvalidDatabaseOperationError("school_profile");
            }
        }

        _schoolProfileManager.ClearCache(request.UserId);

        return uploadingResult;
    }
}
