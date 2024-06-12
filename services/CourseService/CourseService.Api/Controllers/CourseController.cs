using CourseService.Application.Course.CreateCourse;
using Shared.Contracts.Errors.Invalid;

namespace CourseService.Api.Controllers;

public class CourseController : BaseController
{
    [HttpPost("[action]/")]
    public async Task<IActionResult> Create()
    {
        var userId = User.Identity?.GetId();
        var userRole = User.Identity?.GetRole();

        if (userId is null || userRole is null)
            return ErrorActionResultHandler.Handle(new InvalidError("user"));

        var request = new CreateCourseCommand { Name = "133232", UserId = (Guid)userId };

        var result = await Mediator.Send(request);

        return Ok("asdsds");
    }
}
