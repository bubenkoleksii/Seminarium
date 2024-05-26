namespace SchoolService.Api.Controllers;

public class SchoolProfileController(IMapper mapper) : BaseController
{
    [HttpPost("[action]/{invitationCode}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolProfileResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(string invitationCode)
    {
        var command = new CreateSchoolProfileCommand(invitationCode);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }
}
