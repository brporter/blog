using NuGet.Packaging.Signing;
using posts.Commands;
using posts.Models;

namespace posts.Services;

public interface IBlogRepository
{
    Task<RepositoryValue<Blog>> GetBlogAsync(string slug);
}

public class BlogRepository : IBlogRepository
{
    private readonly GetBlogCommand _getBlogCommand;

    public BlogRepository(GetBlogCommand getBlogCommand)
    {
        _getBlogCommand = getBlogCommand;
    }

    public async Task<RepositoryValue<Blog>> GetBlogAsync(string slug)
        => await _getBlogCommand.ExecuteAsync(slug);
}