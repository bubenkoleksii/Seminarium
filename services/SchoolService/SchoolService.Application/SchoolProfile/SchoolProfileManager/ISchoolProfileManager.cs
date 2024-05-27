namespace SchoolService.Application.SchoolProfile.SchoolProfileManager;

public interface ISchoolProfileManager
{
    Task<Either<Domain.Entities.SchoolProfile, Error>> CreateSchoolAdminProfile(Invitation invitation, CreateSchoolProfileCommand command);

    Task<Either<Domain.Entities.SchoolProfile, Error>> CreateTeacherProfile(Invitation invitation, CreateSchoolProfileCommand command);
}
