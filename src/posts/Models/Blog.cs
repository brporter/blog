namespace posts.Models;

public enum State
{
    Disabled = 0,
    Enabled = 1,
    Created,
    Creating,
    Updating,
    Conflict,
    Deleted
}

public readonly record struct RepositoryOperation<T>
{
    readonly T Value;
    readonly State State;
}

public record Blog(int BlogId, string Slug, string Title, string Description, DateTime Created, DateTime Modified, bool Enabled, int UserId);