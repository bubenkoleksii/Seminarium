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
