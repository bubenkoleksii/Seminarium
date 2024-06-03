namespace SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfile;

public class DeleteSchoolProfileCommandHandler : IRequestHandler<DeleteSchoolProfileCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext;

    private readonly IFilesManager _filesManager;

    private readonly ISchoolProfileManager _schoolProfileManager;

    public DeleteSchoolProfileCommandHandler(
        ICommandContext commandContext,
        IFilesManager filesManager,
        ISchoolProfileManager schoolProfileManager)
    {
        _commandContext = commandContext;
        _filesManager = filesManager;
        _schoolProfileManager = schoolProfileManager;
    }

    public async Task<Option<Error>> Handle(DeleteSchoolProfileCommand request, CancellationToken cancellationToken)
    {
        var entity = await _commandContext.SchoolProfiles
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            return Option<Error>.None;

        var profiles = await _commandContext.SchoolProfiles
            .AsNoTracking()
            .Include(p => p.Group)
            .Include(p => p.School)
            .Where(p => p.UserId == entity.UserId)
            .ToListAsync(cancellationToken);

        var deleterProfile = await _schoolProfileManager.GetActiveProfile(request.UserId);
        var cannotDeleteExternalDeleter = deleterProfile is not { Type: SchoolProfileType.SchoolAdmin } ||
                                          deleterProfile.Type != SchoolProfileType.ClassTeacher ||
                                          (deleterProfile.Type == SchoolProfileType.SchoolAdmin &&
                                           entity.SchoolId != deleterProfile.SchoolId) ||
                                          (deleterProfile.Type == SchoolProfileType.ClassTeacher &&
                                           entity.GroupId != deleterProfile.GroupId);
        var cannotOwnDelete = profiles.Find(p => p.Id == entity.Id) == null;

        if (cannotOwnDelete && cannotDeleteExternalDeleter)
            return new InvalidError("school_profile");

        _schoolProfileManager.ClearCache(entity.UserId);

        try
        {
            _commandContext.SchoolProfiles.Remove(entity);
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the school profile with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("school_profile");
        }

        await _filesManager.DeleteImageIfExists(entity.Img);

        if (profiles.Count > 2 && entity.IsActive)
        {
            var newActiveProfile = profiles
                .Where(p => p.Id != request.Id)
                .MaxBy(p => p.CreatedAt);

            newActiveProfile!.IsActive = true;

            _commandContext.SchoolProfiles.Update(newActiveProfile);

            try
            {
                await _commandContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while changing the active school profile with ID {@SchoolId}.", request.Id);

                return new InvalidDatabaseOperationError("school_profile");
            }
        }

        return Option<Error>.None;
    }
}
