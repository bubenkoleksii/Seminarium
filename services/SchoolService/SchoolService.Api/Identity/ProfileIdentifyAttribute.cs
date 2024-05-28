namespace SchoolService.Api.Identity;

public class ProfileIdentifyAttribute(string[] allowedProfileTypes, bool allowedForAdmin = false) : ActionFilterAttribute
{
    private const string HeaderKeyName = "SchoolProfileId";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.User.Identity?.GetId();
        var userRole = context.HttpContext.User.Identity?.GetRole();

        if (userId is null || userRole is null)
        {
            var problemDetail = CreateProblemDetail(
                "Unable to identify the user and their role.",
                "unauthorized",
                StatusCodes.Status401Unauthorized,
                "user"
            );
            SetResult(context, problemDetail);

            return;
        }

        if (userRole == Constants.AdminRole)
        {
            if (!allowedForAdmin)
            {
                var problemDetail = CreateProblemDetail(
                    "This resource is not available to the administrator.",
                    "forbidden",
                    StatusCodes.Status403Forbidden,
                    "user_role"
                );
                SetResult(context, problemDetail);
            }

            return;
        }

        var schoolProfileIdRetrieved =
            context.HttpContext.Request.Headers.TryGetValue(HeaderKeyName, out var schoolProfileId);
        var isSchoolProfileIdValid = Guid.TryParse(schoolProfileId, out var schoolProfileGuid);

        if (!schoolProfileIdRetrieved || !isSchoolProfileIdValid)
        {
            var problemDetail = CreateProblemDetail(
                "Unable to identify the user school profile.",
                "forbidden",
                StatusCodes.Status403Forbidden,
                "school_profile"
            );
            SetResult(context, problemDetail);

            return;
        }

        var schoolProfileManager = context.HttpContext.RequestServices.GetRequiredService<ISchoolProfileManager>();

        var profiles = schoolProfileManager.GetProfiles((Guid)userId).Result;
        var currentProfile = profiles?.FirstOrDefault(p => p.Id == schoolProfileGuid);

        if (currentProfile is null)
        {
            var problemDetail = CreateProblemDetail(
                "User school profile not found.",
                "forbidden",
                StatusCodes.Status403Forbidden,
                "school_profile"
            );
            SetResult(context, problemDetail);

            return;
        }

        if (!currentProfile.IsActive || !SchoolProfileTypeExists(allowedProfileTypes, currentProfile.Type))
        {
            var problemDetail = CreateProblemDetail(
                "Incorrect user profile.",
                "invalid_school_profile",
                StatusCodes.Status403Forbidden,
                "school_profile"
            );
            SetResult(context, problemDetail);
        }
    }

    private static ProblemDetails CreateProblemDetail(string detail, string title, int statusCode, string extensionKey)
    {
        return new ProblemDetails
        {
            Detail = detail,
            Title = title,
            Status = statusCode,
            Type = "access_denied",
            Extensions =
            {
                { "params", new List<string> { extensionKey } }
            }
        };
    }

    private static void SetResult(ActionExecutingContext context, ProblemDetails problemDetail)
    {
        context.Result = new ObjectResult(problemDetail)
        {
            StatusCode = problemDetail.Status
        };
    }

    public static bool SchoolProfileTypeExists(string[] allowedProfiles, string profile) =>
        allowedProfiles.Exists(s => s.Contains(profile));
}
