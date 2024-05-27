namespace SchoolService.Api.Controllers;

public class GroupController(IMapper mapper) : BaseController
{
    [Authorize]
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GroupResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest groupRequest)
    {
        var command = mapper.Map<CreateGroupCommand>(groupRequest);
        command.SchoolId = Guid.Parse("87ed8d9e-5684-4224-8747-c5332cff14d7");

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<GroupResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }
}
