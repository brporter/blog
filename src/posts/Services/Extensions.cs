using System.Configuration;
using Microsoft.Data.SqlClient;
using posts.Commands;
using System.Data.Common;
using StackExchange.Redis;

namespace posts.Services;

public static class Extensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddSingleton<DbProviderFactory>((_) => SqlClientFactory.Instance);
        services.AddSingleton<DataConnectionBroker>();
        services.AddSingleton<IPostRepository, PostRepository>();
        services.AddSingleton<IBlogRepository, BlogRepository>();

        return services;
    }

    public static IServiceCollection AddRedisCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>((services) =>
        {
            var config = services.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("RedisCache");

            if (!string.IsNullOrEmpty(connectionString))
            {
                return ConnectionMultiplexer.Connect(connectionString);
            }

            throw new InvalidOperationException("Unable to connect to the Redis cache instance.");
        });

        services.AddSingleton<ICache, Cache>();

        return services;
    }
}
