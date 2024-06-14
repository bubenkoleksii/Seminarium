namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.DeletePracticalLessonItem;

public class DeletePracticalLessonItemCommandHandler(
    ISchoolProfileAccessor schoolProfileAccessor,
    IAttachmentManager attachmentManager,
    ICommandContext commandContext
    ) : IRequestHandler<DeletePracticalLessonItemCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly IAttachmentManager _attachmentManager = attachmentManager;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    public async Task<Option<Error>> Handle(DeletePracticalLessonItemCommand request, CancellationToken cancellationToken)
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

        var lessonItem = await _commandContext.PracticalLessonItems
            .Include(item => item.Lesson)
            .Where(item => item.Id == request.Id)
            .FirstOrDefaultAsync(CancellationToken.None);
        if (lessonItem == null)
            return Option<Error>.None;

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(lessonItem.Lesson.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        try
        {
            _commandContext.PracticalLessonItems.Remove(lessonItem);

            await _attachmentManager.DeleteAttachments(lessonItem.Attachments, cancellationToken);

            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the practical lesson item with ID {@PracticalLessonItemId}.", request.Id);

            return new InvalidDatabaseOperationError("lesson");
        }

        return Option<Error>.None;
    }
}
