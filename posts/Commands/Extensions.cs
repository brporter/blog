namespace posts.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.AddSingleton<GetPostCommand>();
        services.AddSingleton<GetPostsCommand>();

        return services;
    }
}
