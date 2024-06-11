namespace SchoolService.Api.Controllers;

public class StudyPeriodController(IMapper mapper) : BaseController
{
    [Authorize(Roles = Constants.UserRole)]
    [HttpGet("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StudyPeriodResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var query = new GetAllStudyPeriodsQuery((Guid)userId);

        var result = await Mediator.Send(query);
        return Ok(mapper.Map<IEnumerable<StudyPeriodResponse>>(result));
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPost("[action]/")]
    [ProfileIdentify([Constants.SchoolAdmin])]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudyPeriodResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateStudyPeriodRequest studyPeriodRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<CreateStudyPeriodCommand>(studyPeriodRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<StudyPeriodResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [ProfileIdentify([Constants.SchoolAdmin])]
    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudyPeriodResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateStudyPeriodRequest studyPeriodRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<UpdateStudyPeriodCommand>(studyPeriodRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);
        return result.Match(
            Left: modelResponse => Ok(mapper.Map<StudyPeriodResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [ProfileIdentify([Constants.SchoolAdmin])]
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

        var command = new DeleteStudyPeriodCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
