namespace SchoolService.Application.GroupNotice.Commands.DeleteGroupNotice;

public class DeleteGroupNoticeCommandHandler(ISchoolProfileManager schoolProfileManager, ICommandContext commandContext)
    : IRequestHandler<DeleteGroupNoticeCommand, Option<Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Option<Error>> Handle(DeleteGroupNoticeCommand request, CancellationToken cancellationToken)
    {
        var notice = await _commandContext.GroupNotices
            .Include(notice => notice.Group)
            .FirstOrDefaultAsync(n => n.Id == request.Id, CancellationToken.None);
        if (notice == null)
            return Option<Error>.None;

        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null)
            return new InvalidError("school_profile");

        var canAdminDelete = (profile.Type == SchoolProfileType.SchoolAdmin && notice.Group.SchoolId == profile.SchoolId)
                || (profile.Type == SchoolProfileType.ClassTeacher && notice.GroupId == profile.GroupId);
        var canOwnDelete = profile.Id == notice.AuthorId;

        if (!canAdminDelete && !canOwnDelete)
            return new InvalidError("school_profile");

        try
        {
            _commandContext.GroupNotices.Remove(notice);
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while deleting the study period with ID {@SchoolId}.", request.Id);
            return new InvalidDatabaseOperationError("group_notice");
        }

        return Option<Error>.None;
    }
}
