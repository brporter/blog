namespace posts.Services;

public interface ICache
{
    Task<Optional<T>> GetValueAsync<T>(string key);

    Task SetValueAsync<T>(string key, T value);

    Task QueuePublishAsync<T>(string channel, T value);

    Task QueueSubscribeAsync<T>(string channel, Func<T, Task> callback);

    Task PublishAsync<T>(string channel, T value);

    Task SubscribeAsync<T>(string channel, Func<T, Task> callback);
}