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

        var request = new CreateCourseCommand { Name = "133232", UserId = (Guid)userId, StudyPeriodId = Guid.Parse("e0195403-58bc-468f-b728-a47f4f92dbb2") };

        var result = await Mediator.Send(request);

        return Ok("asdsds");
    }
}
