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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JoiningRequestResponse>))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllJoiningRequestsParams filterParams)
    {
        var query = mapper.Map<GetAllJoiningRequestsQuery>(filterParams);

        var result = await Mediator.Send(query);

        return Ok(mapper.Map<IEnumerable<JoiningRequestResponse>>(result));
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
}
