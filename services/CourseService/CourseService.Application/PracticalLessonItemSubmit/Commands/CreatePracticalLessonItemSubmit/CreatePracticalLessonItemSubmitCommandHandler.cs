namespace CourseService.Application.PracticalLessonItemSubmit.Commands.CreatePracticalLessonItemSubmit;

public class CreatePracticalLessonItemSubmitCommandHandler(
    IAttachmentManager attachmentManager,
    ISchoolProfileAccessor schoolProfileAccessor,
    ICommandContext commandContext,
    IMapper mapper
    ) : IRequestHandler<CreatePracticalLessonItemSubmitCommand, Either<PracticalLessonItemSubmitModelResponse, Error>>
{
    private readonly IAttachmentManager _attachmentManager = attachmentManager;

    private readonly ISchoolProfileAccessor _schoolProfileAccessor = schoolProfileAccessor;

    private readonly ICommandContext _commandContext = commandContext;

    private readonly IMapper _mapper = mapper;

    public async Task<Either<PracticalLessonItemSubmitModelResponse, Error>> Handle(CreatePracticalLessonItemSubmitCommand request, CancellationToken cancellationToken)
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

        var practicalLessonItem = await _commandContext.PracticalLessonItems
            .Include(item => item.Lesson)
            .FirstOrDefaultAsync(item => item.Id == request.PracticalLessonItemId, CancellationToken.None);

        if (practicalLessonItem == null)
            return new InvalidError("practice_item");

        var existedEntities = await _commandContext.PracticalLessonItemSubmits
            .Where(s => s.StudentId == activeProfile.Id && s.PracticalLessonItemId == practicalLessonItem.Id)
            .ToListAsync();

        if (practicalLessonItem.Attempts.HasValue && practicalLessonItem.Attempts.Value <= existedEntities.Count)
            return new InvalidError("max_attempts_count");

        if (practicalLessonItem.Deadline.HasValue)
        {
            var utcDeadline = practicalLessonItem.Deadline.Value.ToUniversalTime();
            var utcNow = DateTime.UtcNow;

            if (utcNow > utcDeadline)
                return new InvalidError("deadline");
        }

        var entity = _mapper.Map<Domain.Entities.PracticalLessonItemSubmit>(request);
        entity.PracticalLessonItem = practicalLessonItem;
        entity.Attempt = (uint)existedEntities.Count + 1;
        entity.Status = PracticalLessonItemSubmitStatus.Submitted;
        entity.StudentId = activeProfile.Id;

        await _commandContext.PracticalLessonItemSubmits.AddAsync(entity, cancellationToken);

        try
        {
            await _commandContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while creating the practical lesson item submit with values {@Request}.", request);
            return new InvalidDatabaseOperationError("lesson");
        }

        var (attachments, attachmentsLinks) = await _attachmentManager.ProcessAttachments(request.Attachments, entity, Constants.TheoryItem);

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

        entity.PracticalLessonItem.Submits = null;
        var practiceLessonItemSubmitModelResponse = _mapper.Map<PracticalLessonItemSubmitModelResponse>(entity);
        practiceLessonItemSubmitModelResponse.Attachments = attachmentsLinks;

        return practiceLessonItemSubmitModelResponse;

    }
}
