namespace CourseService.Application.Common.SchoolProfile;

public interface ISchoolProfileAccessor
{
    public Task<Either<SchoolProfileContract, Error>> GetActiveSchoolProfile
        (GetActiveSchoolProfileRequest request, CancellationToken cancellationToken);

    public Task<Option<Error>> ValidateTeacherByCourse(Guid courseId, Guid teacherId);

    public Task<Option<Error>> ValidateStudentGroupByCourse(Guid courseId, Guid groupId);
}
