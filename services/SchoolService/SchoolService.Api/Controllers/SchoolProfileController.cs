namespace SchoolService.Api.Controllers;

public class SchoolProfileController(IMapper mapper, IOptions<Shared.Contracts.Options.FileOptions> fileOptions) : BaseController
{
    [Authorize]
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SchoolProfileResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var queryUserId = userRole == Constants.AdminRole ? null : userId;
        var query = new GetOneSchoolProfileQuery(
            id,
            queryUserId
        );

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpGet("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SchoolProfileResponse>))]
    public async Task<IActionResult> Get()
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var query = new GetUserSchoolProfilesQuery((Guid)userId);

        var result = await Mediator.Send(query);
        return Ok(mapper.Map<IEnumerable<SchoolProfileResponse>>(result));
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolProfileResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSchoolProfileRequest request)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = mapper.Map<CreateSchoolProfileCommand>(request);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateSchoolProfileRequest schoolProfileRequest)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var command = mapper.Map<UpdateSchoolProfileCommand>(schoolProfileRequest);
        command.UserId = (Guid)userId;

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPost("[action]/")]
    [ProfileIdentify([Constants.SchoolAdmin, Constants.ClassTeacher, Constants.Student], true)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ParentInvitation([FromBody] CreateParentInvitationRequest request)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var query = new CreateParentInvitationCommand(request.ChildId, (Guid)userId);

        var result = await Mediator.Send(query);
        return result.Match(
            Left: response => CreatedAtAction(nameof(Create), response),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPatch("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = new ActivateSchoolProfileCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);
        return result.Match(
            Left: modelResponse => Ok(mapper.Map<SchoolProfileResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpPatch("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileSuccess))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Image(Guid id, [FromForm] IFormFile image)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var maxAllowedSizeInMb = fileOptions.Value.MaxSizeInMb;
        var urlExpirationInMin = fileOptions.Value.UrlExpirationInMin;

        var mappingStreamResult = FileMapper.GetStreamIfValid(image, isImage: true, maxAllowedSizeInMb);
        if (mappingStreamResult.IsRight)
        {
            var error = (Error)mappingStreamResult;
            return ErrorActionResultHandler.Handle(error);
        }
        var stream = (Stream)mappingStreamResult;

        var command = new SetSchoolProfileImageCommand(id, (Guid)userId, image.FileName, stream, urlExpirationInMin);
        var result = await Mediator.Send(command);

        return result.Match(
            Left: Ok,
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = Constants.UserRole)]
    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Image(Guid id)
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_id"));

        if (userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user_role"));

        var command = new DeleteSchoolProfileImageCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }

    [Authorize(Roles = Constants.UserRole)]
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

        var command = new DeleteSchoolProfileCommand(id, (Guid)userId);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
