namespace SchoolService.Application.Group.Commands.DeleteGroupImage;

public class DeleteGroupImageCommandHandler : IRequestHandler<DeleteGroupImageCommand, Option<Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    public DeleteGroupImageCommandHandler(
        ICommandContext commandContext,
        IFilesManager filesManager,
        ISchoolProfileManager schoolProfileManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<Option<Error>> Handle(DeleteGroupImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.Groups
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.GroupId, "group");

        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        var canModify = (profile?.Type == SchoolProfileType.SchoolAdmin && entity.SchoolId == profile.SchoolId) ||
                        (profile?.Type == SchoolProfileType.ClassTeacher && entity.ClassTeacherId == profile.Id);

        if (profile is null || !canModify)
            return new InvalidError("school_profile");

        var deletingResult = await _filesManager.DeleteFileIfExists(entity.Img);
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
