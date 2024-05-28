namespace SchoolService.Application.School.Commands.SetSchoolImage;

public class SetSchoolImageCommandHandler : IRequestHandler<SetSchoolImageCommand, Either<FileSuccess, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IFilesManager _filesManager;

    public SetSchoolImageCommandHandler(
        ICommandContext commandContext,
        ISchoolProfileManager schoolProfileManager,
        IFilesManager filesManager)
    {
        _commandContext = commandContext;
        _schoolProfileManager = schoolProfileManager;
        _filesManager = filesManager;
    }

    public async Task<Either<FileSuccess, Error>> Handle(SetSchoolImageCommand request, CancellationToken cancellationToken)
    {
        var validationError =
            await _schoolProfileManager.ValidateSchoolProfileBySchool(request.UserId, request.SchoolId);

        if (validationError.IsSome)
            return (Error)validationError;

        var entity = await _commandContext.Schools
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == request.SchoolId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.SchoolId, "school");

        var deletingResult = await _filesManager.DeleteImageIfExists(entity.Img);
        if (deletingResult.IsSome)
        {
            Log.Error("An error occurred while deleting image the school with values {@SchoolId} {@FileName}.", entity.Id, entity.Img);
            return (Error)deletingResult;
        }

        var newFileName = $"{Guid.NewGuid()}_school_{request.Name}";
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
                Log.Error(exception, "An error occurred while setting image the school with values {@SchoolId} {@FileName}.", entity.Id, request.Name);

                return new InvalidDatabaseOperationError("school");
            }
        }

        return uploadingResult;
    }
}
