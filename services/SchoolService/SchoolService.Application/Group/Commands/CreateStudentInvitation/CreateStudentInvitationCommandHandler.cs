namespace SchoolService.Application.Group.Commands.CreateStudentInvitation;

public class CreateStudentInvitationCommandHandler : IRequestHandler<CreateStudentInvitationCommand, Either<string, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IConfiguration _configuration;

    private readonly IInvitationManager _invitationManager;

    public CreateStudentInvitationCommandHandler(
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

    public async Task<Either<string, Error>> Handle(CreateStudentInvitationCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        if (profile is null || profile.Type is not SchoolProfileType.SchoolAdmin or SchoolProfileType.ClassTeacher)
            return new InvalidError("school_profile");

        var group = await _queryContext.Groups.FindAsync(request.GroupId);
        if (group is null)
            return new NotFoundByIdError(request.GroupId, "group");

        switch (profile.Type)
        {
            case SchoolProfileType.SchoolAdmin when group.SchoolId != profile.SchoolId:
                return new InvalidError("school_profile");
            case SchoolProfileType.ClassTeacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileByGroup(request.UserId, request.GroupId);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
        }

        var invitationExpiration = _configuration.GetValue<int>("InvitationExpirationInHours:Student");
        var invitation = new Invitation(group.Id, SchoolProfileType.Student, DateTime.UtcNow.AddHours(invitationExpiration));
        var invitationCode = _invitationManager.GenerateInvitationCode(invitation);
        var encodedInvitationCode = Uri.EscapeDataString(invitationCode);

        var clientUrl = _configuration["ClientUrl"]!;
        var link = $"{clientUrl}uk/u/school-profile/create/student/{encodedInvitationCode}";

        return link;
    }
}
