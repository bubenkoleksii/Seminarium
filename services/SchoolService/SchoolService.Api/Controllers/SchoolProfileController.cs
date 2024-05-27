using SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfileImage;

namespace SchoolService.Api.Controllers;

public class SchoolProfileController(IMapper mapper, IOptions<Shared.Contracts.Options.FileOptions> fileOptions) : BaseController
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

        var command = new SetSchoolProfileImageCommand(id, image.FileName, stream, urlExpirationInMin);
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
        var command = new DeleteSchoolProfileImageCommand(id);

        var result = await Mediator.Send(command);

        return result.Match(
            None: Accepted,
            Some: ErrorActionResultHandler.Handle);
    }
}
