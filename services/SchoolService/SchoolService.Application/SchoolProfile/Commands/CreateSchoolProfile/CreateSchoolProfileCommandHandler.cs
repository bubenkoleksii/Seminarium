namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommandHandler : IRequestHandler<CreateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IInvitationManager _invitationManager;

    private readonly IMapper _mapper;

    public CreateSchoolProfileCommandHandler(ICommandContext commandContext, IInvitationManager invitationManager, IMapper mapper)
    {
        _commandContext = commandContext;
        _invitationManager = invitationManager;
        _mapper = mapper;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(CreateSchoolProfileCommand request,
        CancellationToken cancellationToken)
    {
        var invitationData = _invitationManager.GetInvitationData(request.InvitationCode);
        if (invitationData.IsRight)
        {
            Log.Error("An error occurred while reading invitation code with values {@InvitationCode}.",
                request.InvitationCode);
            return (Error)invitationData;
        }

        var profile = ((InvitationSerializationData)invitationData).Type switch
        {
            SchoolProfileType.SchoolAdmin =>
                await CreateSchoolAdminProfile((InvitationSerializationData)invitationData, request),
            SchoolProfileType.Teacher =>
                await CreateTeacherProfile((InvitationSerializationData)invitationData, request),
            _ => new InvalidError("type")
        };

        if (profile.IsRight)
        {
            Log.Error("An error occurred while building school profile with values {@Request}.",
                request);
            return (Error)profile;
        }

        await _commandContext.SchoolProfiles.AddAsync((Domain.Entities.SchoolProfile)profile, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while saving school profile with values {@Profile}.",
                (Domain.Entities.SchoolProfile)profile);
        }

        var schoolProfileResponse = _mapper.Map<SchoolProfileModelResponse>((Domain.Entities.SchoolProfile)profile);
        return schoolProfileResponse;
    }

    private async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateSchoolAdminProfile(
        InvitationSerializationData invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        var school = await _commandContext.Schools.FindAsync(invitation.SourceId);
        if (school == null)
            return new InvalidError("school_id");

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);
        profile.School = school;
        profile.Type = invitation.Type;

        return profile;
    }

    private async Task<Either<Domain.Entities.SchoolProfile, Error>> CreateTeacherProfile(
        InvitationSerializationData invitation, CreateSchoolProfileCommand command)
    {
        if (DateTime.UtcNow > invitation.Expired)
            return new InvalidError("invitation");

        if (command.TeachersLessonsPerCycle is null)
            return new InvalidError("lessons_per_cycle");

        var school = await _commandContext.Schools.FindAsync(invitation.SourceId);
        if (school == null)
            return new InvalidError("school_id");

        var profile = _mapper.Map<Domain.Entities.SchoolProfile>(command);

        var data = new TeacherSerializationData(command.TeachersExperience,
            command.TeachersEducation,
            command.TeachersQualification,
            (uint)command.TeachersLessonsPerCycle
        );

        profile.School = school;
        profile.Type = invitation.Type;
        profile.Data = JsonConvert.SerializeObject(data);

        return profile;
    }
}
