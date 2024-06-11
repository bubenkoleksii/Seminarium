namespace SchoolService.Application.Group.Commands.UpdateGroup;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Either<GroupModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly ICommandContext _commandContext;

    private readonly IMapper _mapper;

    public UpdateGroupCommandHandler(ISchoolProfileManager schoolProfileManager, ICommandContext commandContext, IMapper mapper)
    {
        _schoolProfileManager = schoolProfileManager;
        _commandContext = commandContext;
        _mapper = mapper;
    }

    public async Task<Either<GroupModelResponse, Error>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        var group = await _commandContext.Groups.FindAsync(request.Id);
        if (group is null)
            return new NotFoundByIdError(request.Id, "group");

        var canModify = (profile?.Type == SchoolProfileType.SchoolAdmin && group.SchoolId == profile.SchoolId) ||
                             (profile?.Type == SchoolProfileType.ClassTeacher && group.ClassTeacherId == profile.Id);

        if (profile is null || !canModify)
            return new InvalidError("school_profile");

        _mapper.Map(request, group);

        _commandContext.Groups.Update(group);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the group with values {@Request}.", request);

            return new InvalidDatabaseOperationError("group");
        }

        var groupModelResponse = _mapper.Map<GroupModelResponse>(group);
        return groupModelResponse;
    }
}
