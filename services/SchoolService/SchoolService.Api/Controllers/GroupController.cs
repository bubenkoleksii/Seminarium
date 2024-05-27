namespace SchoolService.Api.Controllers;

public class GroupController(IMapper mapper, IOptions<Shared.Contracts.Options.FileOptions> fileOptions) : BaseController
{
    [Authorize]
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GroupResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateGroupRequest groupRequest)
    {
        var command = mapper.Map<CreateGroupCommand>(groupRequest);
        command.SchoolId = Guid.Parse("87ed8d9e-5684-4224-8747-c5332cff14d7");

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<GroupResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize]
    [HttpPatch("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileSuccess))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Image(Guid id, [FromForm] IFormFile image)
    {
        var maxAllowedSizeInMb = fileOptions.Value.MaxSizeInMb;
        var urlExpirationInMin = fileOptions.Value.UrlExpirationInMin;

        var mappingStreamResult = FileMapper.GetStreamIfValid(image, isImage: true, maxAllowedSizeInMb);
        if (mappingStreamResult.IsRight)
        {
            var error = (Error)mappingStreamResult;
            return ErrorActionResultHandler.Handle(error);
        }
        var stream = (Stream)mappingStreamResult;

        var command = new SetGroupImageCommand(id, image.FileName, stream, urlExpirationInMin);
        var result = await Mediator.Send(command);

        return result.Match(
            Left: Ok,
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize]
    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Image(Guid id)
    {
        var command = new DeleteGroupImageCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
