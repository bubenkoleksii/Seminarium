namespace SchoolService.Api.Controllers;

public class JoiningRequestController(IMapper mapper) : BaseController
{
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JoiningRequestResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var query = new GetOneJoiningRequestQuery(id);

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<JoiningRequestResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpGet("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllJoiningRequestsResponse))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllJoiningRequestsParams filterParams)
    {
        var query = mapper.Map<GetAllJoiningRequestsQuery>(filterParams);

        var result = await Mediator.Send(query);

        return Ok(mapper.Map<GetAllJoiningRequestsResponse>(result));
    }

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(JoiningRequestResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateJoiningRequest joiningRequest)
    {
        var command = mapper.Map<CreateJoiningRequestCommand>(joiningRequest);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<JoiningRequestResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPatch("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RejectJoiningRequestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectJoiningRequest joiningRequest)
    {
        var command = new RejectJoiningRequestCommand(id, joiningRequest.Message);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<RejectJoiningRequestResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }
}
