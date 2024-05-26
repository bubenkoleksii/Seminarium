namespace SchoolService.Api.Controllers;

public class SchoolProfileController : BaseController
{
    [HttpPost("[action]/{invitationCode}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolProfileResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(string invitationCode)
    {
        return Ok(invitationCode);
    }
}
