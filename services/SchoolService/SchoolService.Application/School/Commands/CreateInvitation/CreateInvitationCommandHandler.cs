namespace SchoolService.Application.School.Commands.CreateInvitation;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Either<string, Error>>
{
    private readonly IQueryContext _queryContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IConfiguration _configuration;

    private readonly IInvitationManager _invitationManager;

    public CreateInvitationCommandHandler(
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

    public async Task<Either<string, Error>> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        var validationError =
            await _schoolProfileManager.ValidateSchoolProfileBySchool(request.UserId, request.SchoolId);

        if (validationError.IsSome)
            return (Error)validationError;

        var entity = await _queryContext.Schools
            .FirstOrDefaultAsync(s => s.Id == request.SchoolId, cancellationToken: cancellationToken);

        if (entity is null)
            return new NotFoundByIdError(request.SchoolId, "school");

        var invitationExpiration = _configuration.GetValue<int>("InvitationExpirationInHours:SchoolAdmin");
        var invitation = new Invitation(entity.Id, SchoolProfileType.SchoolAdmin, DateTime.UtcNow.AddHours(invitationExpiration));
        var invitationCode = _invitationManager.GenerateInvitationCode(invitation);
        var encodedInvitationCode = Uri.EscapeDataString(invitationCode);

        var clientUrl = _configuration["ClientUrl"]!;
        var link = $"{clientUrl}uk/school-profile/create/school_admin/{encodedInvitationCode}";

        return link;
    }
}
