namespace SchoolService.Application.Group.Commands.DeleteGroup;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Option<Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    public DeleteGroupCommandHandler(ICommandContext commandContext, IFilesManager filesManager, ISchoolProfileManager schoolProfileManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<Option<Error>> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        var group = await _commandContext.Groups.FindAsync(request.Id);
        if (group is null)
            return new NotFoundByIdError(request.Id, "group");

        var canModify = (profile?.Type == SchoolProfileType.SchoolAdmin && group.SchoolId == profile.SchoolId) ||
                        (profile?.Type == SchoolProfileType.ClassTeacher && group.ClassTeacherId == profile.Id);

        if (profile is null || !canModify)
            return new InvalidError("school_profile");

        try
        {
            _commandContext.Groups.Remove(group);
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the school profile with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("school_profile");
        }

        await _filesManager.DeleteFileIfExists(group.Img);
        return Option<Error>.None;
    }
}
