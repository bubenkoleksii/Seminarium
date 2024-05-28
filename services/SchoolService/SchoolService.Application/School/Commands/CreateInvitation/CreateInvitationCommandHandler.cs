namespace SchoolService.Application.School.Commands.CreateInvitation;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Either<string, Error>>
{
    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IConfiguration _configuration;

    private readonly IInvitationManager _invitationManager;

    public CreateInvitationCommandHandler(
        ISchoolProfileManager schoolProfileManager,
        IConfiguration configuration,
        IInvitationManager invitationManager)
    {
        _schoolProfileManager = schoolProfileManager;
        _configuration = configuration;
        _invitationManager = invitationManager;
    }

    public async Task<Either<string, Error>> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        var validationError = await ValidateSchoolProfileAsync(request);
        if (validationError.IsSome)
            return (Error)validationError;

        var invitationExpiration = _configuration.GetValue<int>("InvitationExpirationInHours:SchoolAdmin");
        var invitation = new Invitation(request.SchoolId, SchoolProfileType.SchoolAdmin, DateTime.UtcNow.AddHours(invitationExpiration));
        var invitationCode = _invitationManager.GenerateInvitationCode(invitation);
        var encodedInvitationCode = Uri.EscapeDataString(invitationCode);

        var clientUrl = _configuration["ClientUrl"]!;
        var link = $"{clientUrl}uk/school-profile/create/school_admin/{encodedInvitationCode}";

        return link;
    }

    private async Task<Option<Error>> ValidateSchoolProfileAsync(CreateInvitationCommand request)
    {
        if (request.UserId is not null)
        {
            var schoolProfile = await _schoolProfileManager.GetActiveProfile((Guid)request.UserId);
            if (schoolProfile?.SchoolId is null)
                return new InvalidError("school_profile");

            var schoolId = (Guid)schoolProfile.SchoolId;
            if (schoolId != request.SchoolId)
                return new InvalidError("school_id");
        }

        return Option<Error>.None;
    }
}
