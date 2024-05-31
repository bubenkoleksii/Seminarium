namespace SchoolService.Application.SchoolProfile.Commands.ActivateSchoolProfile;

public class ActivateSchoolProfileCommandHandler : IRequestHandler<ActivateSchoolProfileCommand, Either<SchoolProfileModelResponse, Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly ISchoolProfileManager _schoolProfileManager;

    public ActivateSchoolProfileCommandHandler(ICommandContext commandContext, ISchoolProfileManager schoolProfileManager)
    {
        _commandContext = commandContext;
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<Either<SchoolProfileModelResponse, Error>> Handle(ActivateSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        var profiles = await _commandContext.SchoolProfiles
            .Where(profile => profile.UserId == request.UserId)
            .ToListAsync(cancellationToken: cancellationToken);

        var profile = profiles.Find(p => p.Id == request.Id);
        if (profile is null)
            return new NotFoundByIdError(request.Id, "school_profile");

        profiles.ForEach(p => p.IsActive = p.Id == request.Id);
        _commandContext.SchoolProfiles.UpdateRange(profiles);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while setting status to active of the school profile with values {@Request}.", request);
            return new InvalidDatabaseOperationError("school_profile");
        }

        var currentProfile = await _schoolProfileManager.CacheProfiles(request.UserId, profile.Id);
        if (currentProfile is null)
            return new InvalidError("user");

        return currentProfile;
    }
}
