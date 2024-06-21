namespace CourseService.Application.Common.SchoolProfile;

public class SchoolProfileAccessor(
    IRequestClient<GetActiveSchoolProfileRequest> schoolProfileClient,
    ICommandContext commandContext
    ) : ISchoolProfileAccessor
{
    private readonly IRequestClient<GetActiveSchoolProfileRequest> _schoolProfileClient = schoolProfileClient;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Either<SchoolProfileContract, Error>> GetActiveSchoolProfile(
        GetActiveSchoolProfileRequest request,
        CancellationToken cancellationToken)
    {
        var getActiveSchoolProfileRequest = new GetActiveSchoolProfileRequest(
            request.UserId,
            [Constants.ClassTeacher, Constants.SchoolAdmin, Constants.Teacher, Constants.SchoolAdmin, Constants.Student]
        );

        try
        {
            var response =
                await _schoolProfileClient.GetResponse<GetActiveSchoolProfileResponse>(getActiveSchoolProfileRequest, cancellationToken);

            if (response.Message.Error == null && response.Message.SchoolProfile == null)
                return new InternalServicesError("school");

            if (response.Message.HasError && response.Message.Error != null)
                return response.Message.Error;

            var activeProfile = response.Message.SchoolProfile;
            if (activeProfile == null)
                return new InvalidError("school_profile");

            return activeProfile;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while sending get active school porfile with values @Request", getActiveSchoolProfileRequest);
            return new InternalServicesError("school");
        }
    }

    public async Task<Option<Error>> ValidateTeacherByCourse(Guid courseId, Guid teacherId)
    {
        var course = await _commandContext.Courses
           .AsNoTracking()
           .Include(course => course.Teachers)
           .FirstOrDefaultAsync(course => course.Id == courseId, CancellationToken.None);

        if (course == null)
            return new InvalidError("course");

        if (course.Teachers is null)
            return new InvalidError("school_profile");

        var isInvalidTeacher = !course.Teachers.Any(t => t.Id == teacherId);
        if (isInvalidTeacher)
            return new InvalidError("school_profile");

        return Option<Error>.None;
    }

    public async Task<Option<Error>> ValidateStudentGroupByCourse(Guid courseId, Guid groupId)
    {
        var course = await _commandContext.Courses
           .AsNoTracking()
           .Include(course => course.Teachers)
           .FirstOrDefaultAsync(course => course.Id == courseId, CancellationToken.None);

        if (course == null)
            return new InvalidError("course");

        if (course.Groups is null)
            return new InvalidError("school_profile");

        var isInvalidGroup = !course.Groups.Any(g => g.Id == groupId);
        if (isInvalidGroup)
            return new InvalidError("school_profile");

        return Option<Error>.None;
    }
}
