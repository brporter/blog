using System.Data.Common;
using posts.Models;
using posts.Services;
using Dapper;

namespace posts.Commands;

public class GetPostCommand
{
    private readonly DataConnectionBroker _broker;

    public GetPostCommand(DataConnectionBroker broker)
    {
        _broker = broker ?? throw new ArgumentNullException(nameof(broker));
    }

    public async Task<Post> ExecuteAsync(int postId)
    {
        using var connection = _broker.GetConnection();
        connection.Open();

        var command = new CommandDefinition(
            "SELECT postid, title, body, created, modified, enabled, userid FROM dbo.posts WHERE postid = @PostId ORDER BY created DESC",
            new
            {
                PostId = postId
            });

        var post = await connection.ExecuteScalarAsync<Post>(command);

        return post;
    }
}