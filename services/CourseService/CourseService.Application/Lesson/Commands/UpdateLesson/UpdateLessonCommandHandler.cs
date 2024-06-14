namespace CourseService.Application.Lesson.Commands.UpdateLesson;

public class UpdateLessonCommandHandler(
    ICommandContext commandContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor
    ) : IRequestHandler<UpdateLessonCommand, Either<LessonModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<LessonModelResponse, Error>> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
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

        var lesson = await _commandContext.Lessons
            .Where(l => l.Id == request.Id)
            .FirstOrDefaultAsync(CancellationToken.None);
        if (lesson == null)
            return new NotFoundByIdError(request.Id, "lesson");

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(lesson.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        _mapper.Map(request, lesson);

        _commandContext.Lessons.Update(lesson);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the course with values {@Request}.", request);

            return new InvalidDatabaseOperationError("course");
        }

        lesson.Course.Lessons = null;

        var lessonModelResponse = _mapper.Map<LessonModelResponse>(lesson);
        return lessonModelResponse;
    }
}
