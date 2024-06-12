namespace CourseService.Application.Common.SchoolProfile;

public interface ISchoolProfileAccessor
{
    public Task<Either<SchoolProfileContract, Error>> GetActiveSchoolProfile
        (GetActiveSchoolProfileRequest request, CancellationToken cancellationToken);
}
