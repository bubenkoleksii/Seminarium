namespace CourseService.Api.Controllers;

[ApiController]
[Route("api/[controller]/")]
[Authorize(Roles = Constants.UserRole)]
public abstract class BaseController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>()!;
}
