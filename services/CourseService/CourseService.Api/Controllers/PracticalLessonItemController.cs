namespace CourseService.Api.Controllers;

public class PracticalLessonItemController(IMapper mapper, IAttachmentHelper attachmentHelper) : BaseController
{
    private readonly IMapper _mapper = mapper;

    private readonly IAttachmentHelper _attachmentHelper = attachmentHelper;

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PracticalLessonItemResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreatePracticalLessonItemRequest createRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<CreatePracticalLessonItemCommand>(createRequest);
        command.UserId = (Guid)userId;

        var convertingAttachmentsResult = _attachmentHelper.ConvertToAttachmentRequests(createRequest.Attachments);
        if (convertingAttachmentsResult.IsRight)
            return ErrorActionResultHandler.Handle((Error)convertingAttachmentsResult);

        command.Attachments = (List<AttachmentModelRequest>)convertingAttachmentsResult;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), _mapper.Map<PracticalLessonItemResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PracticalLessonItemResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdatePracticalLessonItemRequest updateRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<UpdatePracticalLessonItemCommand>(updateRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(_mapper.Map<PracticalLessonItemResponse>(modelResponse)),
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

        var command = new DeletePracticalLessonItemCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}