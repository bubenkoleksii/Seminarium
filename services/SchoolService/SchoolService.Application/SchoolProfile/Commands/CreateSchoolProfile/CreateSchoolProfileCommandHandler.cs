namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommandHandler : IRequestHandler<CreateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    public CreateSchoolProfileCommandHandler(ICommandContext commandContext)
    {
        _commandContext = commandContext;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(CreateSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        await _commandContext.SchoolProfiles.ToListAsync();

        var schoolProfileResponse = new SchoolProfileModelResponse(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, null, request.InvitationCode, null, null, null, null, null);
        return schoolProfileResponse;
    }
}
