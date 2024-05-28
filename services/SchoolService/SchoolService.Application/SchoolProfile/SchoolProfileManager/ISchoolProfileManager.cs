namespace SchoolService.Application.SchoolProfile.SchoolProfileManager;

public interface ISchoolProfileManager
{
    public Task<Either<Domain.Entities.SchoolProfile, Error>> CreateSchoolAdminProfile(Invitation invitation, CreateSchoolProfileCommand command);

    public Task<Either<Domain.Entities.SchoolProfile, Error>> CreateTeacherProfile(Invitation invitation, CreateSchoolProfileCommand command);

    public Task<bool> CacheProfiles(Guid userId, Guid currentProfileId);

    public Task<IEnumerable<SchoolProfileModelResponse>?> GetProfiles(Guid userId);

    public Task<SchoolProfileModelResponse?> GetActiveProfile(Guid userId);

    public void ClearCache(Guid userId);
}
