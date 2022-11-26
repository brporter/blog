using NuGet.Packaging.Signing;
using posts.Commands;
using posts.Models;

namespace posts.Services;

public class RepositoryValue<T>
{
    private readonly T? _value;

    private RepositoryValue(T? value)
    {
        _value = value;
    }
    
    public T Value => _value!;

    public bool HasValue => _value != null;

    public static implicit operator T(RepositoryValue<T> r) => r.Value;
    public static implicit operator RepositoryValue<T>(T v) => new(v);
}

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