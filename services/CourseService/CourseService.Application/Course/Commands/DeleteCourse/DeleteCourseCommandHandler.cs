namespace CourseService.Application.Course.Commands.DeleteCourse;

public class DeleteCourseCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext
    ) : IRequestHandler<DeleteCourseCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    public async Task<Option<Error>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
                         UserId: request.UserId,
                         AllowedProfileTypes: [Constants.Teacher]
                     );

        var retrievingActiveProfileResult =
            await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return new InvalidError("school_profile");

        var course = await _commandContext.Courses.FirstOrDefaultAsync(c => c.Id == request.Id, CancellationToken.None);
        if (course is null)
            return new NotFoundByIdError(request.Id, "course");

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(course.Id, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        try
        {
            _commandContext.Courses.Remove(course);
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the course with ID {@SchoolId}.", request.Id);

            return new InvalidDatabaseOperationError("course");
        }

        return Option<Error>.None;
    }
}
