using Microsoft.Extensions.FileProviders.Physical;
using NuGet.Packaging.Signing;
using posts.Commands;
using posts.Models;

namespace posts.Services;

public interface IBlogRepository
{
    Task<Optional<Blog>> GetBlogAsync(string slug);
}

public class BlogRepository : IBlogRepository
{
    private readonly GetBlogCommand _getBlogCommand;
    private readonly ICache _cache;

    public BlogRepository(ICache cache, GetBlogCommand getBlogCommand)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _getBlogCommand = getBlogCommand;
    }

    public async Task<Optional<Blog>> GetBlogAsync(string slug)
    {
        var value = await _cache.GetValueAsync<Blog>($"blog:{slug}");

        if (value.HasValue)
            return value;

        value = await _getBlogCommand.ExecuteAsync(slug);

        if (value.HasValue)
            await _cache.SetValueAsync($"blog:{slug}", value.Value);

        return value;
    }
}