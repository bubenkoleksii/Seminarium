﻿using Shared.Contracts.Errors.Invalid;

namespace SchoolService.Api.Controllers;

public class SchoolProfileController(IMapper mapper) : BaseController
{
    [Authorize]
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolProfileResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSchoolProfileRequest request)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null or Constants.AdminRole)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = mapper.Map<CreateSchoolProfileCommand>(request);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }
}
