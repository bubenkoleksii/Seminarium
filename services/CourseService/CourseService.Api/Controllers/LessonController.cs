namespace CourseService.Api.Controllers;

public class LessonController(IMapper mapper) : BaseController
{
    private readonly IMapper _mapper = mapper;

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LessonResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateLessonRequest createRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<CreateLessonCommand>(createRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), _mapper.Map<LessonResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LessonResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateLessonRequest updateRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = _mapper.Map<UpdateLessonCommand>(updateRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(_mapper.Map<LessonResponse>(modelResponse)),
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

        var command = new DeleteLessonCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
