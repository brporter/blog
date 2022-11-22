using posts.Models;

namespace posts.HostedServices;

public static class Extensions
{
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<DatabaseInitializationService>();

        return services;
    }
}