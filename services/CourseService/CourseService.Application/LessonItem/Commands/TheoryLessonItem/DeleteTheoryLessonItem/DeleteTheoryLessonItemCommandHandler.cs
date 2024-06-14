namespace CourseService.Application.LessonItem.Commands.TheoryLessonItem.DeleteTheoryLessonItem;

public class DeleteTheoryLessonItemCommandHandler(
       ISchoolProfileAccessor schoolProfileAccessor,
       IAttachmentManager attachmentManager,
       ICommandContext commandContext
       ) : IRequestHandler<DeleteTheoryLessonItemCommand, Option<Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly IAttachmentManager _attachmentManager = attachmentManager;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    public async Task<Option<Error>> Handle(DeleteTheoryLessonItemCommand request, CancellationToken cancellationToken)
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

        var lessonItem = await _commandContext.TheoryLessonItems
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
            _commandContext.TheoryLessonItems.Remove(lessonItem);

            await _attachmentManager.DeleteAttachments(lessonItem.Attachments, cancellationToken);

            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while deleting the theory lesson item with ID {@TheoryLessonItemId}.", request.Id);

            return new InvalidDatabaseOperationError("lesson");
        }

        return Option<Error>.None;
    }
}
