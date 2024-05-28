namespace SchoolService.Api.Identity;

public static class Config
{
    public static void AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["IdentityUrl"]!;
                options.Audience = "SeminariumApp";
                options.RequireHttpsMetadata = false;
            });
    }
}

public static class Constants
{
    public const string AdminRole = "admin";

    public const string SchoolAdmin = "school_admin";

    public const string Teacher = "teacher";

    public const string ClassTeacher = "class_teacher";

    public const string Student = "student";

    public const string Parent = "parent";
}
