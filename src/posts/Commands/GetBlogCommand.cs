using System.Data;
using Dapper;
using posts.Models;
using posts.Services;

namespace posts.Commands;

public class GetBlogCommand
{
    private readonly DataConnectionBroker _broker;

    public GetBlogCommand(DataConnectionBroker broker)
    {
        _broker = broker ?? throw new ArgumentNullException(nameof(broker));
    }

    public async Task<Blog> ExecuteAsync(string slug)
    {
        using var connection = _broker.GetConnection();
        connection.Open();

        var command = new CommandDefinition(
            "dbo.GetBlogBySlug",
            new
            {
                Slug = slug
            },
            commandType: CommandType.StoredProcedure
        );

        var blog = await connection.QuerySingleAsync<Blog>(command);

        return blog;
    }
}