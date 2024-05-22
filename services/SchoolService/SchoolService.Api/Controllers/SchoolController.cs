namespace SchoolService.Api.Controllers;

public class SchoolController(IMapper mapper, IOptions<Shared.Contracts.Options.FileOptions> fileOptions) : BaseController
{
    [Authorize]
    [HttpGet("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var query = new GetOneSchoolQuery(id);

        var result = await Mediator.Send(query);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<SchoolResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllSchoolsResponse))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSchoolsParams filterParams)
    {
        var query = mapper.Map<GetAllSchoolsQuery>(filterParams);

        var result = await Mediator.Send(query);

        return Ok(mapper.Map<GetAllSchoolsResponse>(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSchoolRequest schoolRequest)
    {
        var command = mapper.Map<CreateSchoolCommand>(schoolRequest);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<SchoolResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [Authorize]
    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateSchoolRequest schoolRequest)
    {
        var command = mapper.Map<UpdateSchoolCommand>(schoolRequest);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<SchoolResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

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

        var command = new SetSchoolImageCommand(id, image.FileName, stream, urlExpirationInMin);
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
        var command = new DeleteSchoolImageCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteSchoolCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
