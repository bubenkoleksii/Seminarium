﻿namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommandHandler : IRequestHandler<CreateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IInvitationManager _invitationManager;

    private readonly IMapper _mapper;

    private readonly ISchoolProfileManager _schoolProfileManager;

    public CreateSchoolProfileCommandHandler(
        ICommandContext commandContext,
        IInvitationManager invitationManager,
        IMapper mapper,
        ISchoolProfileManager schoolProfileManager)
    {
        _commandContext = commandContext;
        _invitationManager = invitationManager;
        _mapper = mapper;
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(CreateSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        var invitationData = _invitationManager.GetInvitationData(request.InvitationCode);
        if (invitationData.IsRight)
        {
            Log.Error("An error occurred while reading invitation code with values {@InvitationCode}.", request.InvitationCode);
            return (Error)invitationData;
        }

        var invitation = (Invitation)invitationData;

        var profile = invitation.Type switch
        {
            SchoolProfileType.SchoolAdmin => await _schoolProfileManager.CreateSchoolAdminProfile(invitation, request),
            SchoolProfileType.Teacher => await _schoolProfileManager.CreateTeacherProfile(invitation, request),
            _ => new InvalidError("type")
        };

        if (profile.IsRight)
        {
            Log.Error("An error occurred while building school profile with values {@Request}.", request);
            return (Error)profile;
        }

        await _commandContext.SchoolProfiles.AddAsync((Domain.Entities.SchoolProfile)profile, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while saving school profile with values {@Profile}.", (Domain.Entities.SchoolProfile)profile);
        }

        var schoolProfileResponse = _mapper.Map<SchoolProfileModelResponse>((Domain.Entities.SchoolProfile)profile);
        return schoolProfileResponse;
    }
}
