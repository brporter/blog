namespace posts.Models;

public record Post(int PostId, string Title, string Body, DateTime Created, DateTime Modified, bool Enabled,
    int UserId);
