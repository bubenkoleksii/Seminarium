namespace SchoolService.Application.School.Commands.CreateSchool;

public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, Either<SchoolModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IMailService _mailService;

    private readonly IMapper _mapper;

    private readonly IConfiguration _configuration;

    private readonly IInvitationManager _invitationManager;

    public CreateSchoolCommandHandler(ICommandContext commandContext,
        IMailService mailService,
        IMapper mapper,
        IConfiguration configuration,
        IInvitationManager invitationManager)
    {
        _commandContext = commandContext;
        _mailService = mailService;
        _mapper = mapper;
        _configuration = configuration;
        _invitationManager = invitationManager;
    }

    public async Task<Either<SchoolModelResponse, Error>> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Domain.Entities.School>(request);

        var joiningRequest = await _commandContext.JoiningRequests.FindAsync(request.JoiningRequestId);
        if (joiningRequest is null)
            return new InvalidError("joining request");

        joiningRequest.Status = JoiningRequestStatus.Approved;
        entity.JoiningRequest = joiningRequest;

        var isAlreadyExists = UniquenessChecker.GetErrorIfAlreadyExists(_commandContext, entity, out var error);
        if (isAlreadyExists)
            return error!;

        await _commandContext.Schools.AddAsync(entity, cancellationToken);
        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the school with values {@Request}.", request);
            return new InvalidDatabaseOperationError("school");
        }

        var invitationExpiration = _configuration.GetValue<int>("InvitationExpirationInHours:SchoolAdmin");
        var invitation = new Invitation(entity.Id, SchoolProfileType.SchoolAdmin, DateTime.UtcNow.AddHours(invitationExpiration));
        var invitationCode = _invitationManager.GenerateInvitationCode(invitation);
        var encodedInvitationCode = Uri.EscapeDataString(invitationCode);

        var clientUrl = _configuration["ClientUrl"]!;
        var link = $"{clientUrl}uk/u/school-profile/create/school_admin/{encodedInvitationCode}";

        try
        {
            await _mailService.SendAsync(
                joiningRequest.RequesterEmail,
                EmailTemplates.AcceptJoiningRequest.Subject,
                EmailTemplates.AcceptJoiningRequest.GetTemplate(joiningRequest.Id, entity.Name, link));
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while send email to {@Email} after accepting joining request.", joiningRequest.RequesterEmail);
        }

        var schoolResponse = _mapper.Map<SchoolModelResponse>(entity);
        return schoolResponse;
    }
}
