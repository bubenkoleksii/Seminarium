using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagementService.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration
            .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
