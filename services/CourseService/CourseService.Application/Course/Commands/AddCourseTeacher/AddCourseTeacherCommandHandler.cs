namespace CourseService.Application.Course.Commands.AddCourseTeacher;

public class AddCourseTeacherCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IRequestClient<GetSchoolProfilesRequest> getSchoolProfilesClient,
    ICommandContext commandContext
    ) : IRequestHandler<AddCourseTeacherCommand, Either<CourseTeacherModelResponse, Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IRequestClient<GetSchoolProfilesRequest> _getSchoolProfilesClient = getSchoolProfilesClient;

    private readonly ICommandContext _commandContext = commandContext;

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
            (activeProfile.Type != Constants.SchoolAdmin && activeProfile.Type != Constants.Teacher))
            return new InvalidError("school_profile");

        var course = await _commandContext.Courses
            .Include(course => course.Teachers)
            .FirstOrDefaultAsync(course => course.Id == request.CourseId, CancellationToken.None);
        if (course == null)
            return new InvalidError("course");

        var getTeachersRequest = new GetSchoolProfilesRequest(Ids: null, UserId: null, GroupId: null, SchoolId: request.SchoolId, Type: null);

        var teachersResponse =
            await _getSchoolProfilesClient.GetResponse<GetSchoolProfilesResponse>(getTeachersRequest, cancellationToken);
        if (teachersResponse.Message.HasError)
            return (Error)teachersResponse;

        if (teachersResponse.Message.Profiles == null || !teachersResponse.Message.Profiles.Any())
            return new NotFoundError("teacher_profiles");

        var filteredTeacher = teachersResponse.Message.Profiles
            .FirstOrDefault(profile => string.Equals(profile.Name, request.Name, StringComparison.OrdinalIgnoreCase) &&
                profile.Type == "teacher");
        if (filteredTeacher == null)
            return new NotFoundError("teacher_profile");

        course.Teachers ??= [];
        var existedTeacher = await _commandContext.CourseTeachers.FindAsync(filteredTeacher.Id);
        if (existedTeacher != null)
        {
            course.Teachers.Add(existedTeacher);
        }
        else
        {
            var newCourseTeacher = new CourseTeacher
            {
                Id = filteredTeacher.Id
            };

            newCourseTeacher.IsCreator = false;

            try
            {
                await _commandContext.CourseTeachers.AddAsync(newCourseTeacher, cancellationToken);
                await _commandContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "An error occurred while creating the course teacher with values {@Request}.", request);
                return new InvalidDatabaseOperationError("course");
            }

            course.Teachers.Add(newCourseTeacher);
        }

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the course teacher with values {@Request}.", request);
            return new InvalidDatabaseOperationError("course");
        }

        return new CourseTeacherModelResponse(filteredTeacher.Id, filteredTeacher.Name, IsCreator: false);
    }
}