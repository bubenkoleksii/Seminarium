﻿namespace SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;

public class CreateSchoolProfileCommandHandler : IRequestHandler<CreateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IInvitationManager _invitationManager;

    private readonly ISchoolProfileManager _schoolProfileManager;

    private readonly IConfiguration _configuration;

    public CreateSchoolProfileCommandHandler(
        ICommandContext commandContext,
        IInvitationManager invitationManager,
        ISchoolProfileManager schoolProfileManager,
        IConfiguration configuration)
    {
        _commandContext = commandContext;
        _invitationManager = invitationManager;
        _schoolProfileManager = schoolProfileManager;
        _configuration = configuration;
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

        var existedProfiles = await _commandContext.SchoolProfiles
            .Where(p => p.UserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        var profilesMaxCount = _configuration.GetValue<int>("MaxProfilesCount");
        if (existedProfiles.Count >= profilesMaxCount)
            return new InvalidError("max_profiles_count");

        var profile = invitation.Type switch
        {
            SchoolProfileType.SchoolAdmin => await _schoolProfileManager.CreateSchoolAdminProfile(invitation, request),
            SchoolProfileType.Teacher => await _schoolProfileManager.CreateTeacherProfile(invitation, request),
            SchoolProfileType.Student => await _schoolProfileManager.CreateStudentProfile(invitation, request),
            SchoolProfileType.Parent => await _schoolProfileManager.CreateParentProfileOrAddChild(invitation, request),
            SchoolProfileType.ClassTeacher => await _schoolProfileManager.CreateClassTeacherProfile(invitation, request),
            _ => new InvalidError("type")
        };

        if (profile.IsRight)
        {
            Log.Error("An error occurred while building school profile with values {@Request}.", request);
            return (Error)profile;
        }

        existedProfiles.ForEach(p => p.IsActive = false);

        _commandContext.SchoolProfiles.UpdateRange(existedProfiles);
        await _commandContext.SchoolProfiles.AddAsync((Domain.Entities.SchoolProfile)profile, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while saving school profile with values {@Profile}.", (Domain.Entities.SchoolProfile)profile);
        }

        var currentProfile = await _schoolProfileManager.CacheProfiles(request.UserId, ((Domain.Entities.SchoolProfile)profile).Id);
        if (currentProfile is null)
            return new InvalidError("user");

        return currentProfile;
    }
}
