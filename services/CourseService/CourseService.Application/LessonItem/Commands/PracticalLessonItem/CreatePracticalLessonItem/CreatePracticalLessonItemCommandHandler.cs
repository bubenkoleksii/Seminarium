namespace CourseService.Application.LessonItem.Commands.PracticalLessonItem.CreatePracticalLessonItem;

public class CreatePracticalLessonItemCommandHandler(
    ICommandContext commandContext,
    IMapper mapper,
    ISchoolProfileAccessor schoolProfileAccessor,
    IAttachmentManager attachmentManager
    ) : IRequestHandler<CreatePracticalLessonItemCommand, Either<PracticalLessonItemModelResponse, Error>>
{
    private readonly ICommandContext _commandContext = commandContext;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly IMapper _mapper = mapper;

    private readonly IAttachmentManager _attachmentManager = attachmentManager;

    public async Task<Either<PracticalLessonItemModelResponse, Error>> Handle(CreatePracticalLessonItemCommand request, CancellationToken cancellationToken)
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

        var lesson = await _commandContext.Lessons.FindAsync(request.LessonId, cancellationToken);
        if (lesson is null)
            return new InvalidError("lesson");

        var teacherValidatingResult = await _schoolProfileAccessor.ValidateTeacherByCourse(lesson.CourseId, activeProfile.Id);
        if (teacherValidatingResult.IsSome)
            return (Error)teacherValidatingResult;

        var author = await _commandContext.CourseTeachers.FindAsync(activeProfile.Id);

        var entity = _mapper.Map<Domain.Entities.PracticalLessonItem>(request);
        entity.Lesson = lesson;
        entity.Author = author;

        await _commandContext.PracticalLessonItems.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the practical lesson item with values {@Request}.", request);
            return new InvalidDatabaseOperationError("lesson");
        }

        var (attachments, attachmentsLinks) = await _attachmentManager.ProcessAttachments(request.Attachments, entity, Constants.PracticeItem);

        await _commandContext.Attachments.AddRangeAsync(attachments, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the list of lesson item attachments with values {@Request}.", attachments);
            return new InvalidDatabaseOperationError("attachment");
        }

        var practicalLessonModelResponse = _mapper.Map<PracticalLessonItemModelResponse>(entity);
        practicalLessonModelResponse.Attachments = attachmentsLinks;
        practicalLessonModelResponse.Author = activeProfile;

        return practicalLessonModelResponse;
    }
}
