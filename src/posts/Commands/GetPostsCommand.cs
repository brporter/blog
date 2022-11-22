using System.Data;
using Dapper;
using posts.Models;
using posts.Services;

namespace posts.Commands;

public class GetPostsCommand
{
    private readonly DataConnectionBroker _broker;

    public GetPostsCommand(DataConnectionBroker broker)
    {
        _broker = broker ?? throw new ArgumentNullException(nameof(broker));
    }

    public async Task<IEnumerable<Post>> ExecuteAsync(int userId, int page)
    {
        using var connection = _broker.GetConnection();
        connection.Open();

        var command = new CommandDefinition(
            "SELECT postid, title, body, created, modified, enabled, userid FROM dbo.posts WHERE userid = @UserId ORDER BY created DESC OFFSET( (@PageNumber - 1) * 10 ) ROWS FETCH NEXT 10 ROWS ONLY",
            new
            {
                UserId = userId,
                PageNumber = page
            });

        var posts = await connection.QueryAsync<Post>(command);

        return posts;
    }

    public async Task<IEnumerable<Post>> ExecuteAsync(string handle, int page)
    {
        using var connection = _broker.GetConnection();
        connection.Open();

        var command = new CommandDefinition(
            "dbo.GetPostsByHandle", new
            {
                Handle = handle,
                PageNumber = page,
                RowsPerPage = 10
            },
            commandType: CommandType.StoredProcedure);

        var posts = await connection.QueryAsync<Post>(command);

        return posts;
    }
}