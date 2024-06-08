namespace SchoolService.Api.Controllers;

public class StudyPeriodController : BaseController
{
    [Authorize(Roles = Constants.UserRole)]
    public IActionResult Create()
    {
        return Ok();
    }
}
