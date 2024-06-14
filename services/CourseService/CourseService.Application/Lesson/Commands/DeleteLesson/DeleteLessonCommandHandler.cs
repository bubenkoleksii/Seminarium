namespace CourseService.Application.Lesson.Commands.DeleteLesson;

public class DeleteLessonCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext
    ) : IRequestHandler<DeleteLessonCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    public async Task<Option<Error>> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
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

        var lesson = await _commandContext.Lessons.FindAsync(request.Id, CancellationToken.None);
        if (lesson == null)
            return Option<Error>.None;

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(lesson.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        try
        {
            _commandContext.Lessons.Remove(lesson);
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the lesson with ID {@LessonId}.", request.Id);

            return new InvalidDatabaseOperationError("lesson");
        }

        return Option<Error>.None;
    }
}
