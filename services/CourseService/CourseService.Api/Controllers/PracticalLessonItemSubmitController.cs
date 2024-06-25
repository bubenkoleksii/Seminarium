namespace CourseService.Api.Controllers;

public class PracticalLessonItemSubmitController(IMapper mapper, IAttachmentHelper attachmentHelper) : BaseController
{
    private readonly IMapper _mapper = mapper;

    private readonly IAttachmentHelper _attachmentHelper = attachmentHelper;

    [HttpGet("[action]/")]
    public async Task<IActionResult> GetTeacherAll([FromQuery] GetTeacherAllPracticalLessonItemSubmitParams filterParams)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var query = new GetAllTeacherPracticalLessonItemsSubmitQuery(filterParams.ItemId, filterParams.Skip, filterParams.Take);

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(_mapper.Map<GetAllPracticalLessonItemSubmitResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpGet("[action]/{studentId}/{practicalItemId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PracticalLessonItemSubmitResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOne(Guid studentId, Guid practicalItemId)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var query = new GetOnePracticalLessonItemSubmitQuery(practicalItemId, studentId);

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(_mapper.Map<PracticalLessonItemSubmitResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPatch("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddResults([FromBody] AddPracticalItemSubmitResultsRequest resultsRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<AddResultsPracticalLessonItemSubmitCommand>(resultsRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle
        );
    }

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PracticalLessonItemSubmitResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreatePracticalLessonItemSubmitRequest createRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<CreatePracticalLessonItemSubmitCommand>(createRequest);
        command.UserId = (Guid)userId;

        var convertingAttachmentsResult = _attachmentHelper.ConvertToAttachmentRequests(createRequest.Attachments);
        if (convertingAttachmentsResult.IsRight)
            return ErrorActionResultHandler.Handle((Error)convertingAttachmentsResult);

        command.Attachments = (List<AttachmentModelRequest>?)convertingAttachmentsResult;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), _mapper.Map<PracticalLessonItemSubmitResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PracticalLessonItemSubmitResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdatePracticalLessonItemSubmitRequest updateRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<UpdatePracticalLessonItemSubmitCommand>(updateRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(_mapper.Map<PracticalLessonItemSubmitResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = new DeletePracticalLessonItemSubmitCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
