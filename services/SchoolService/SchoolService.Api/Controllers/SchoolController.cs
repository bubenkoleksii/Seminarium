namespace SchoolService.Api.Controllers;

public class SchoolController(IMapper mapper, IOptions<ImageOptions> imageOptions) : BaseController
{
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

    [HttpPost("[action]/")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
    public async Task<IActionResult> Create([FromBody] CreateSchoolRequest schoolRequest)
    {
        var command = mapper.Map<CreateSchoolCommand>(schoolRequest);

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => CreatedAtAction(nameof(Create), mapper.Map<SchoolResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPut("[action]/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SchoolResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
    public async Task<IActionResult> Update([FromForm] UpdateSchoolRequest schoolRequest)
    {
        var command = mapper.Map<UpdateSchoolCommand>(schoolRequest);

        if (schoolRequest.Img is not null)
        {
            var maxAllowedSizeInMb = imageOptions.Value.MaxSizeInMb;
            var mappingResult = ImageMapper.GetStreamIfValid(schoolRequest.Img, maxAllowedSizeInMb);

            if (mappingResult.IsRight)
            {
                var error = (Error)mappingResult;
                return ErrorActionResultHandler.Handle(error);
            }

            command.Img = (Stream)mappingResult;
        }

        var result = await Mediator.Send(command);

        return result.Match(
            Left: modelResponse => Ok(mapper.Map<SchoolResponse>(modelResponse)),
            Right: ErrorActionResultHandler.Handle
        );
    }

    [HttpPatch("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(Guid id)
    {
        var command = new ArchiveSchoolCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }

    [HttpPatch("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
    public async Task<IActionResult> Unarchive(Guid id)
    {
        var command = new UnarchiveSchoolCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }

    [HttpDelete("[action]/{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteSchoolCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
