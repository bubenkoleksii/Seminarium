using Microsoft.AspNetCore.Mvc.Filters;

namespace SchoolManagementService.Errors;

public class ConvertToProblemDetailsFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        throw new NotImplementedException();
    }
}
