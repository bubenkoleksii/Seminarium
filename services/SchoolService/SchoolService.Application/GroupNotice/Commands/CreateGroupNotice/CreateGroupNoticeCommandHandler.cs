namespace SchoolService.Application.GroupNotice.Commands.CreateGroupNotice;

public class CreateGroupNoticeCommandHandler(ISchoolProfileManager schoolProfileManager, IMapper mapper, ICommandContext commandContext)
    : IRequestHandler<CreateGroupNoticeCommand, Either<GroupNoticeModelResponse, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager = schoolProfileManager;

    private readonly IMapper _mapper = mapper;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Either<GroupNoticeModelResponse, Error>> Handle(CreateGroupNoticeCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        if (profile is null)
            return new InvalidError("school_profile");

        var group = await _commandContext.Groups.FindAsync(request.GroupId, CancellationToken.None);
        if (group is null)
            return new InvalidError("group_id");

        var validationResult = await ValidateProfile(profile.Type, profile.UserId, group);
        if (validationResult.IsSome)
            return (Error)validationResult;

        var entity = _mapper.Map<Domain.Entities.GroupNotice>(request);
        entity.Group = group;
        entity.AuthorId = profile.Id;

        await _commandContext.GroupNotices.AddAsync(entity);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the group notice with values {@Request}.", request);
            return new InvalidDatabaseOperationError("group");
        }

        var noticeResponse = _mapper.Map<GroupNoticeModelResponse>(entity);
        noticeResponse.Author = profile;
        return noticeResponse;
    }

    private async Task<Option<Error>> ValidateProfile(SchoolProfileType type, Guid userId, Domain.Entities.Group group)
    {
        switch (type)
        {
            case SchoolProfileType.SchoolAdmin or SchoolProfileType.Teacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileBySchool(userId, group.SchoolId);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Student:
                {
                    var validationError = await _schoolProfileManager.ValidateSchoolProfileByGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.ClassTeacher:
                {
                    var validationError = await _schoolProfileManager.ValidateClassTeacherSchoolProfileByGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Parent:
                {
                    var validationError = await _schoolProfileManager.ValidateParentProfileByChildGroup(userId, group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
        }

        return Option<Error>.None;
    }
}
