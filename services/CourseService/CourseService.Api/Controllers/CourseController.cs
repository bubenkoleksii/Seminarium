namespace CourseService.Api.Controllers;

public class CourseController(IMapper mapper) : BaseController
{
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var query = new GetOneCourseQuery(id, (Guid)userId);

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<CourseResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpGet("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllCoursesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCoursesParams filterParams)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var query = mapper.Map<GetAllCoursesQuery>(filterParams);
        query.UserId = (Guid)userId;

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<GetAllCoursesResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseGroupResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddGroup([FromBody] AddCourseGroupRequest courseGroupRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = new AddCourseGroupCommand(
            (Guid)userId,
            courseGroupRequest.Name,
            courseGroupRequest.CourseId,
            courseGroupRequest.SchoolId
        );

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<CourseGroupResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseTeacherResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddTeacher([FromBody] AddCourseTeacherRequest courseTeacherRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = new AddCourseTeacherCommand(
            (Guid)userId,
            courseTeacherRequest.Name,
            courseTeacherRequest.CourseId,
            courseTeacherRequest.SchoolId
        );

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<CourseTeacherResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }


    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCourseRequest createRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<CreateCourseCommand>(createRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<CourseResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateCourseRequest updateRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<UpdateCourseCommand>(updateRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<CourseResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCourseGroup(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = new DeleteCourseGroupCommand(
            (Guid)userId,
            id
        );

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }

    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCourseTeacher(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = new DeleteCourseTeacherCommand(
            (Guid)userId,
            id
        );

        var result = await Mediator.Send(command);

        return result.Match(
            None: () => Accepted(),
            Some: error => ErrorActionResultHandler.Handle(error));
    }

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

        var command = new DeleteCourseCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
