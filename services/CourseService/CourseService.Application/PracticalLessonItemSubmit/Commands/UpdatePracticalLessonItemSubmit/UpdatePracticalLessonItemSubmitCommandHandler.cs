namespace CourseService.Application.PracticalLessonItemSubmit.Commands.UpdatePracticalLessonItemSubmit;

public class UpdatePracticalLessonItemSubmitCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext,
    IMapper mapper
    ) : IRequestHandler<UpdatePracticalLessonItemSubmitCommand, Either<PracticalLessonItemSubmitModelResponse, Error>>
{
    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly ICommandContext _commandContext = commandContext;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<PracticalLessonItemSubmitModelResponse, Error>> Handle(UpdatePracticalLessonItemSubmitCommand request, CancellationToken cancellationToken)
    {
        var getActiveProfileRequest = new GetActiveSchoolProfileRequest(
                    UserId: request.UserId,
                    AllowedProfileTypes: [Constants.Student]
        );

        var retrievingActiveProfileResult = await _schoolProfileAccessor.GetActiveSchoolProfile(getActiveProfileRequest, cancellationToken);
        if (retrievingActiveProfileResult.IsRight)
            return (Error)retrievingActiveProfileResult;

        var activeProfile = (SchoolProfileContract)retrievingActiveProfileResult;
        if (activeProfile == null)
            return new InvalidError("school_profile");

        var practicalLessonItemSubmit = await _commandContext.PracticalLessonItemSubmits
            .Include(submit => submit.Attachments)
            .Where(submit => submit.Id == request.Id && submit.StudentId == activeProfile.Id)
            .FirstOrDefaultAsync(CancellationToken.None);

        if (practicalLessonItemSubmit == null)
            return new NotFoundByIdError(request.Id, "practical_lesson_item_submit");

        var practicalLessonItem = await _commandContext.PracticalLessonItems
            .Include(item => item.Lesson)
            .FirstOrDefaultAsync(item => item.Id == practicalLessonItemSubmit.PracticalLessonItemId, CancellationToken.None);

        if (practicalLessonItem == null)
            return new InvalidError("practical_lesson_item");

        var studentValidatingResult =
            await _schoolProfileAccessor.ValidateStudentGroupByCourse(practicalLessonItem.Lesson.CourseId, activeProfile.Id);
        if (studentValidatingResult.IsSome)
            return (Error)studentValidatingResult;

        if (practicalLessonItemSubmit.Attachments?.Count == 0 && request.Text == null)
            return new InvalidError("practical_lesson_item_submit");

        if (practicalLessonItemSubmit.Status != PracticalLessonItemSubmitStatus.Submitted)
            return new InvalidError("practical_lesson_item_submit_status");

        if (practicalLessonItem.Deadline.HasValue)
        {
            var utcDeadline = practicalLessonItem.Deadline.Value.ToUniversalTime();
            var utcNow = DateTime.UtcNow;

            if (utcNow > utcDeadline)
                return new InvalidError("deadline");
        }

        _mapper.Map(request, practicalLessonItemSubmit);

        _commandContext.PracticalLessonItemSubmits.Update(practicalLessonItemSubmit);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while updating the practical lesson item submit with values {@Request}.", request);

            return new InvalidDatabaseOperationError("practical_lesson_item_submit");
        }

        practicalLessonItemSubmit.PracticalLessonItem.Submits = null;

        var practicalLessonItemSubmitResponse = _mapper.Map<PracticalLessonItemSubmitModelResponse>(practicalLessonItemSubmit);
        return practicalLessonItemSubmitResponse;
    }
}
