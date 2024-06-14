namespace CourseService.Application.Lesson.Commands.CreateLesson;

public class CreateLessonCommandHandler(
    ICommandContext commandContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor
    ) : IRequestHandler<CreateLessonCommand, Either<LessonModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<LessonModelResponse, Error>> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
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

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(request.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        if (request.Date.HasValue)
        {
            var existedEntity = await _commandContext.Lessons
                .Where(l => l.Date == request.Date.Value)
                .FirstOrDefaultAsync();

            if (existedEntity != null)
                return new AlreadyExistsError("lesson");
        }

        var entity = _mapper.Map<Domain.Entities.Lesson>(request);

        await _commandContext.Lessons.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the lesson with values {@Request}.", request);
            return new InvalidDatabaseOperationError("lesson");
        }

        entity.Course.Lessons = null;

        var lessonResponse = _mapper.Map<LessonModelResponse>(entity);
        return lessonResponse;
    }
}
