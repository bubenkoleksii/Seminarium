namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.UpdateTheoryLessonItem;

public class UpdateTheoryLessonItemCommandHandler(
        ICommandContext commandContext,
        IMapper mapper,
        ISchoolProfileAccessor schoolProfileAccessor
        ) : IRequestHandler<UpdateTheoryLessonItemCommand, Either<TheoryLessonItemModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;
    private readonly IMapper _mapper = mapper;

    public async Task<Either<TheoryLessonItemModelResponse, Error>> Handle(UpdateTheoryLessonItemCommand request, CancellationToken cancellationToken)
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

        var theoryLessonItem = await _commandContext.TheoryLessonItems
            .Include(item => item.Lesson)
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(CancellationToken.None);

        if (theoryLessonItem == null)
            return new NotFoundByIdError(request.Id, "theory_lesson_item");

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(theoryLessonItem.Lesson.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        _mapper.Map(request, theoryLessonItem);

        _commandContext.TheoryLessonItems.Update(theoryLessonItem);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the theory lesson item with values {@Request}.", request);

            return new InvalidDatabaseOperationError("theory_lesson_item");
        }

        theoryLessonItem.Lesson.LessonItems = null;

        var theoryLessonItemModelResponse = _mapper.Map<TheoryLessonItemModelResponse>(theoryLessonItem);
        return theoryLessonItemModelResponse;
    }
}
