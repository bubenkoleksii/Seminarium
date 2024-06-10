namespace SchoolService.Api.Controllers;

public class GroupNoticeController(IMapper mapper) : BaseController
{
    [Authorize(Roles = Constants.UserRole)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllGroupNoticesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] GetAllGroupNoticesParams filterParams)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var query = mapper.Map<GetAllGroupNoticesQuery>(filterParams);
        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<GetAllGroupNoticesResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GroupNoticeResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateGroupNoticeRequest groupNoticeRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<CreateGroupNoticeCommand>(groupNoticeRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<GroupNoticeResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }decimal 

    [Authorize(Roles = Constants.UserRole)]
    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupNoticeResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateGroupNoticeRequest groupNoticeRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<UpdateGroupNoticeCommand>(groupNoticeRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);
        return result.Match(
            Left: modelResponse => Ok(mapper.Map<GroupNoticeResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPatch("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupNoticeResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crucial([FromBody] ChangeGroupNoticeCrucialRequest crucialRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = new ChangeGroupNoticeCrucialCommand(crucialRequest.Id, (Guid)userId, crucialRequest.IsCrucial);
        var result = await Mediator.Send(command);
        return result.Match(
            Left: modelResponse => Ok(mapper.Map<GroupNoticeResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
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

        var command = new DeleteGroupNoticeCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
