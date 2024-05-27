namespace SchoolService.Application.SchoolProfile.SchoolProfileManager;

public class SchoolProfileManager : ISchoolProfileManager
{
    public Task<Either<Domain.Entities.SchoolProfile, Error>> CreateSchoolAdminProfile(Invitation invitation, CreateSchoolProfileCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<Either<Domain.Entities.SchoolProfile, Error>> CreateTeacherProfile(Invitation invitation, CreateSchoolProfileCommand command)
    {
        throw new NotImplementedException();
    }
}
