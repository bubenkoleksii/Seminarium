namespace SchoolService.Api.Controllers;

public class JoiningRequestController(IMapper mapper) : BaseController
{
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
