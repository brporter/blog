using Microsoft.Data.SqlClient;
using posts.Commands;
using System.Data.Common;

namespace posts.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<DbProviderFactory>((_) => SqlClientFactory.Instance);
        services.AddSingleton<DataConnectionBroker>();
        services.AddSingleton<IPostRepository, PostRepository>();
        services.AddSingleton<IBlogRepository, BlogRepository>();

        return services;
    }
}
