namespace SchoolService.Application.GroupNotice.Commands.UpdateGroupNotice;

public class UpdateGroupNoticeCommandHandler(ISchoolProfileManager schoolProfileManager, IMapper mapper, ICommandContext commandContext)
    : IRequestHandler<UpdateGroupNoticeCommand, Either<GroupNoticeModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IMapper _mapper = mapper;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Either<GroupNoticeModelResponse, Error>> Handle(UpdateGroupNoticeCommand request, CancellationToken cancellationToken)
    {
        var notice = await _commandContext.GroupNotices
            .Include(notice => notice.Group)
            .FirstOrDefaultAsync(n => n.Id == request.Id, CancellationToken.None);
        if (notice == null)
            return new NotFoundByIdError(request.Id, "notice");

        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null)
            return new InvalidError("school_profile");

        var canAdminUpdate = (profile.Type == SchoolProfileType.SchoolAdmin && notice.Group.SchoolId == profile.SchoolId)
            || (profile.Type == SchoolProfileType.ClassTeacher && notice.GroupId == profile.GroupId);
        var canOwnUpdate = profile.Id == notice.AuthorId;

        if (!(canAdminUpdate && canOwnUpdate))
            return new InvalidError("school_profile");

        _mapper.Map(request, notice);

        _commandContext.GroupNotices.Update(notice);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the notice with values {@Request}.", request);

            return new InvalidDatabaseOperationError("group_notice");
        }

        var noticeModelResponse = _mapper.Map<GroupNoticeModelResponse>(notice);
        return noticeModelResponse;
    }
}
