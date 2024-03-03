namespace SchoolManagementService.Mappings.Parsers;

public static class EnumParser
{
    public static T ParseToEnum<T>(this string value) where T : struct, Enum
    {
        if (Enum.TryParse(value, true, out T result))
            return result;

        throw new ArgumentException($"Cannot parse {value} as {typeof(T).Name}");
    }

    public static string ParseToString<T>(this T value) where T : struct, Enum => nameof(value).ToSnakeCase();

    private static string ToSnakeCase(this string str)
    {
        return string.Join("_", string.Concat(string.Join("_", str.Split(Array.Empty<char>(),
                        StringSplitOptions.RemoveEmptyEntries))
                    .Select(c => char.IsUpper(c)
                        ? $"_{c}".ToLower()
                        : $"{c}"))
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries));
    }
}
