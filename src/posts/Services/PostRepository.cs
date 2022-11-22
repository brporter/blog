using posts.Commands;
using posts.Models;

namespace posts.Services;

public interface IPostRepository
{
    Task<Post> GetPostAsync(int postId);
    Task<IEnumerable<Post>> GetPostsAsync(int userId, int page);
    Task<IEnumerable<Post>> GetPostsAsync(string handle, int page);
}

public class PostRepository : IPostRepository
{
    private readonly GetPostCommand _getPostCommand;
    private readonly GetPostsCommand _getPostsCommand;

    public PostRepository(GetPostCommand getOneCommand, GetPostsCommand getManyCommand)
    {
        _getPostCommand = getOneCommand ?? throw new ArgumentNullException(nameof(getOneCommand));
        _getPostsCommand = getManyCommand ?? throw new ArgumentNullException(nameof(getManyCommand));
    }

    public async Task<Post> GetPostAsync(int postId)
    {
        var post = await _getPostCommand.ExecuteAsync(postId);

        return post;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(int userId, int page)
    {
        var posts = await _getPostsCommand.ExecuteAsync(userId, page);

        return posts;
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(string handle, int page)
    {
        var posts = await _getPostsCommand.ExecuteAsync(handle, page);

        return posts;
    }
}
