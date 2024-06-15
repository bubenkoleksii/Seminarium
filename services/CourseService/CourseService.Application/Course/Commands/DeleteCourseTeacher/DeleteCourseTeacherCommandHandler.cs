namespace CourseService.Application.Course.Commands.DeleteCourseTeacher;

public class DeleteCourseTeacherCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext
    ) : IRequestHandler<DeleteCourseTeacherCommand, Option<Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Option<Error>> Handle(DeleteCourseTeacherCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
            UserId: request.UserId,
            AllowedProfileTypes: [Constants.Teacher, Constants.SchoolAdmin, Constants.ClassTeacher]
        );

        var retrievingActiveProfileResult =
            await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null || activeProfile.SchoolId == null)
            return new InvalidError("school_profile");

        var course = await _commandContext.Courses
            .Include(c => c.Teachers)
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, CancellationToken.None);
        if (course is null || course.Teachers == null)
            return new NotFoundByIdError(request.Id, "course");

        if (activeProfile.Type != Constants.Teacher && activeProfile.Type != Constants.SchoolAdmin)
            return new InvalidError("school_profile");

        if (activeProfile.Type == Constants.Teacher)
        {
            var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(course.Id, activeProfile.Id);
            if (teacherValidatingResult.IsSome)
                return (Error)teacherValidatingResult;
        }

        var courseTeacher = await _commandContext.CourseTeachers.FindAsync(request.Id, CancellationToken.None);
        if (courseTeacher is null)
            return new NotFoundByIdError(request.Id, "course_teacher");

        if (!course.Teachers.Any(t => t.Id == courseTeacher.Id))
            return Option<Error>.None;

        course.Teachers = course.Teachers.Where(t => t.Id != courseTeacher.Id).ToList();

        _commandContext.Courses.Update(course);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the course teacher with ID {@Id}.", request.Id);

            return new InvalidDatabaseOperationError("course_teacher");
        }

        return Option<Error>.None;
    }
}