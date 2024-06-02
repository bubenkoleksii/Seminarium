namespace SchoolService.Application.Group.Commands.CreateClassTeacherInvitation;

public class CreateClassTeacherInvitationCommandHandler : IRequestHandler<CreateClassTeacherInvitationCommand, Either<string, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IConfiguration _configuration;

    private readonly IInvitationManager _invitationManager;

    public CreateClassTeacherInvitationCommandHandler(
        IQueryContext queryContext,
        ISchoolProfileManager schoolProfileManager,
        IConfiguration configuration,
        IInvitationManager invitationManager)
    {
        _queryContext = queryContext;
        _schoolProfileManager = schoolProfileManager;
        _configuration = configuration;
        _invitationManager = invitationManager;
    }

    public async Task<Either<string, Error>> Handle(CreateClassTeacherInvitationCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        if (profile?.Type is not SchoolProfileType.SchoolAdmin)
            return new InvalidError("school_profile");

        var group = await _queryContext.Groups.FindAsync(request.GroupId);
        if (group is null)
            return new NotFoundByIdError(request.GroupId, "group");

        if (profile.SchoolId != group.SchoolId)
            return new InvalidError("school_profile");

        var invitationExpiration = _configuration.GetValue<int>("InvitationExpirationInHours:ClassTeacher");
        var invitation = new Invitation(group.Id, SchoolProfileType.ClassTeacher, DateTime.UtcNow.AddHours(invitationExpiration));
        var invitationCode = _invitationManager.GenerateInvitationCode(invitation);
        var encodedInvitationCode = Uri.EscapeDataString(invitationCode);

        var clientUrl = _configuration["ClientUrl"]!;
        var link = $"{clientUrl}uk/u/school-profile/create/class_teacher/{encodedInvitationCode}";

        return link;
    }
}
