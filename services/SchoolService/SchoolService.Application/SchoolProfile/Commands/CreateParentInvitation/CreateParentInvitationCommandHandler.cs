namespace SchoolService.Application.SchoolProfile.Commands.CreateParentInvitation;

public class CreateParentInvitationCommandHandler : IRequestHandler<CreateParentInvitationCommand, Either<string, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IConfiguration _configuration;

    private readonly IInvitationManager _invitationManager;

    public CreateParentInvitationCommandHandler(
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

    public async Task<Either<string, Error>> Handle(CreateParentInvitationCommand request, CancellationToken cancellationToken)
    {
        var profile = await _schoolProfileManager.GetActiveProfile(request.UserId);

        if (profile is null || profile.Type is SchoolProfileType.Parent or SchoolProfileType.Teacher)
            return new InvalidError("school_profile");

        var child = await _queryContext.SchoolProfiles
            .Include(c => c.Group)
            .FirstOrDefaultAsync(c => c.Id == request.ChildId, cancellationToken);

        if (child?.Group is null)
            return new InvalidError("child");

        switch (profile.Type)
        {
            case SchoolProfileType.SchoolAdmin when child.Group?.SchoolId != profile.SchoolId:
                return new InvalidError("school_profile");
            case SchoolProfileType.ClassTeacher:
                {
                    var validationError =
                        await _schoolProfileManager.ValidateSchoolProfileByGroup(request.UserId, child.Group.Id);

                    if (validationError.IsSome)
                        return (Error)validationError;
                    break;
                }
            case SchoolProfileType.Student when profile.Id != request.ChildId:
                return new InvalidError("school_profile");
        }

        var invitationExpiration = _configuration.GetValue<int>("InvitationExpirationInHours:Parent");
        var invitation = new Invitation(child.Id, SchoolProfileType.Parent, DateTime.UtcNow.AddHours(invitationExpiration));
        var invitationCode = _invitationManager.GenerateInvitationCode(invitation);
        var encodedInvitationCode = Uri.EscapeDataString(invitationCode);

        var clientUrl = _configuration["ClientUrl"]!;
        var link = $"{clientUrl}uk/u/school-profile/create/parent/{encodedInvitationCode}";

        return link;
    }
}
