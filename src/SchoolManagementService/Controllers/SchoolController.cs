using SchoolManagementService.Core.Application.School.Commands.CreateSchool;
using SchoolManagementService.Core.Domain.Errors;
using SchoolManagementService.Models.School;

namespace SchoolManagementService.Controllers;

public class SchoolController(IMapper mapper) : BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> Create([FromBody] CreateSchoolRequest schoolRequest)
    {
        var command = mapper.Map<CreateSchoolCommand>(schoolRequest);

        var result = await Mediator.Send(command);

        return result.Match<IActionResult>(
            Left: modelResponse => Ok(mapper.Map<SchoolResponse>(modelResponse)),
            Right: BadRequest
        );
    }
}
