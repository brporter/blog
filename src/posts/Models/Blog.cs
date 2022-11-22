namespace posts.Models;

public record Blog(int BlogId, string Slug, string Title, string Description, DateTime Created, DateTime Modified, bool Enabled, int UserId);