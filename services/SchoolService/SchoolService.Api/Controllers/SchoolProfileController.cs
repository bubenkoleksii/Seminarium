namespace SchoolService.Api.Controllers;

public class SchoolProfileController(IMapper mapper) : BaseController
{
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolProfileResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSchoolProfileRequest request)
    {
        var command = mapper.Map<CreateSchoolProfileCommand>(request);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }
}
