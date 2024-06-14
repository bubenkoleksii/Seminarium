namespace CourseService.Application.PracticalLessonItemSubmit.Commands.DeletePracticalLessonItemSubmit;

public class DeletePracticalLessonItemSubmitCommandHandler(
    IAttachmentManager attachmentManager,
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext
    ) : IRequestHandler<DeletePracticalLessonItemSubmitCommand, Option<Error>>
{
    private readonly IAttachmentManager _attachmentManager = attachmentManager;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly ICommandContext _commandContext = commandContext;

    public async Task<Option<Error>> Handle(DeletePracticalLessonItemSubmitCommand request, CancellationToken cancellationToken)
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

        var practicalLessonItemSubmit = await _commandContext.PracticalLessonItemSubmits.FindAsync(request.Id, CancellationToken.None);
        if (practicalLessonItemSubmit == null)
            return Option<Error>.None;

        var practicalLessonItem = await _commandContext.PracticalLessonItems
            .Include(item => item.Lesson)
            .FirstOrDefaultAsync(item => item.Id == practicalLessonItemSubmit.PracticalLessonItemId, CancellationToken.None);

        if (practicalLessonItem == null)
            return new InvalidError("practice_item");

        var studentValidatingResult =
            await _schoolProfileAccessor.ValidateStudentGroupByCourse(practicalLessonItem.Lesson.CourseId, activeProfile.Id);
        if (studentValidatingResult.IsSome)
            return (Error)studentValidatingResult;

        try
        {
            _commandContext.PracticalLessonItemSubmits.Remove(practicalLessonItemSubmit);

            await _attachmentManager.DeleteAttachments(practicalLessonItemSubmit.Attachments, cancellationToken);

            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the practical lesson item submit with ID {@Id}.", request.Id);

            return new InvalidDatabaseOperationError("practical_lesson_item_submit");
        }

        return Option<Error>.None;
    }
}
