namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.UpdatePracticalLessonItem;

public class UpdatePracticalLessonItemCommandHandler(
    ICommandContext commandContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor
    ) : IRequestHandler<UpdatePracticalLessonItemCommand, Either<PracticalLessonItemModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<PracticalLessonItemModelResponse, Error>> Handle(UpdatePracticalLessonItemCommand request, CancellationToken cancellationToken)
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

        var practicalLessonItem = await _commandContext.PracticalLessonItems
            .Include(item => item.Lesson)
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(CancellationToken.None);

        if (practicalLessonItem == null)
            return new NotFoundByIdError(request.Id, "practical_lesson_item");

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(practicalLessonItem.Lesson.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        _mapper.Map(request, practicalLessonItem);

        _commandContext.PracticalLessonItems.Update(practicalLessonItem);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the practical lesson item with values {@Request}.", request);

            return new InvalidDatabaseOperationError("practical_lesson_item");
        }

        practicalLessonItem.Lesson.LessonItems = null;

        var practicalLessonItemModelResponse = _mapper.Map<PracticalLessonItemModelResponse>(practicalLessonItem);
        return practicalLessonItemModelResponse;
    }
}
