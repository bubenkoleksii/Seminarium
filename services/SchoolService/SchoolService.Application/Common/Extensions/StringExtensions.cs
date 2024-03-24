namespace SchoolService.Application.Common.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string a, string? b)
        => string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
}
