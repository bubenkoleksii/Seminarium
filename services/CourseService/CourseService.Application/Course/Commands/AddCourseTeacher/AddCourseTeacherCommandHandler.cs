namespace CourseService.Application.Course.Commands.AddCourseTeacher;

public class AddCourseTeacherCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    ICommandContext commandContext
    ) : IRequestHandler<AddCourseTeacherCommand, Either<CourseTeacherModelResponse, Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly ICommandContext _commandContext = commandContext;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    public async Task<Either<CourseTeacherModelResponse, Error>> Handle(AddCourseTeacherCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher, Constants.ClassTeacher, Constants.SchoolAdmin]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null ||
            activeProfile.Type != Constants.SchoolAdmin || activeProfile.Type != Constants.Teacher)
            return new InvalidError("school_profile");

        var course = await _commandContext.Courses
            .Include(course => course.Teachers)
            .FirstOrDefaultAsync(course => course.Id == request.CourseId, CancellationToken.None);
        if (course == null)
            return new InvalidError("course");

        var getTeachersRequest = new GetSchoolProfilesRequest(null, null, null, request.SchoolId, null);
        var getTeachersResponse = await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getTeachersRequest, cancellationToken);

        if (getTeachersResponse.Message.Profiles == null)
            return new NotFoundError("teacher_profiles");

        var teacherProfile = getTeachersResponse.Message.Profiles
            .FirstOrDefault(profile => string.Equals(profile.Name, request.Name, StringComparison.OrdinalIgnoreCase) &&
                 profile.Type == "teacher"
            );
        if (teacherProfile == null)
            return new NotFoundError("teacher_profile");

        var courseTeacher = new CourseTeacher
        {
            Id = teacherProfile.Id,
            IsCreator = false
        };
        course.Teachers ??= [];
        course.Teachers.Add(courseTeacher);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the course teacher with values {@Request}.", request);
            return new InvalidDatabaseOperationError("course_teacher");
        }

        return new CourseTeacherModelResponse(teacherProfile.Id, teacherProfile.Name, IsCreator: false);
    }
}
