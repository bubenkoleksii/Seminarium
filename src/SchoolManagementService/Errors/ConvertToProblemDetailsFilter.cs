using SchoolManagementService.Core.Domain.Errors;

namespace SchoolManagementService.Errors;

public class ConvertToProblemDetailsFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult { Value: Error error } result)
            return;

        var problemDetails = new ProblemDetails
        {
            Detail = error.Detail,
            Title = error.Title,
            Status = result.StatusCode,
            Type = error.Type,
            Extensions =
            {
                { "params", error.Params }
            }
        };

        context.Result = new ObjectResult(problemDetails) { StatusCode = result.StatusCode };
    }
}
