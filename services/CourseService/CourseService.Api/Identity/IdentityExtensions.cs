namespace CourseService.Api.Identity;

public static class IdentityExtensions
{
    public static Guid? GetId(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;

        var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null)
            return null;

        return Guid.Parse(claim.Value);
    }

    public static string? GetRole(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;

        var claim = claimsIdentity?.FindFirst(ClaimTypes.Role);
        return claim?.Value;
    }
}