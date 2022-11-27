using System.Text.Json;
using StackExchange.Redis;

namespace posts.Services;

public class Cache
    : ICache
{
    private readonly ILogger<Cache> _logger;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    
    public Cache(ILogger<Cache> logger, IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
    }

    public async Task<RepositoryValue<T>> GetValueAsync<T>(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var value = await db.StringGetAsync(key);

        if (!value.IsNullOrEmpty)
        {
            var serializedData = (string)value!;

            if (!string.IsNullOrEmpty(serializedData))
            {
                return JsonSerializer.Deserialize<T>(serializedData);
            }
        }

        return RepositoryValue<T>.Empty;
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var serializedValue = JsonSerializer.Serialize(value);

        await db.StringSetAsync(key, serializedValue);
    }

    public async Task QueuePublishAsync<T>(string channel, T value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var sub = _connectionMultiplexer.GetSubscriber();
        var serializedValue = JsonSerializer.Serialize(value);

        await db.ListLeftPushAsync(channel, serializedValue);
        await sub.PublishAsync(channel, string.Empty);
    }

    public async Task QueueSubscribeAsync<T>(string channel, Func<T, Task> callback)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var sub = _connectionMultiplexer.GetSubscriber();
        
        (await sub.SubscribeAsync(channel)).OnMessage(async channelMessage =>
        {
            var serializedMessage = await db.ListLeftPopAsync(channel);

            if (!serializedMessage.IsNullOrEmpty)
            {
                var o = JsonSerializer.Deserialize<T>(serializedMessage!);

                if (o != null)
                {
                    await callback(o);
                }
            }
        });
    }

    public async Task PublishAsync<T>(string channel, T value)
    {
        var sub = _connectionMultiplexer.GetSubscriber();
        var serializedValue = JsonSerializer.Serialize(value);
        await sub.PublishAsync(channel, serializedValue);
    }

    public async Task SubscribeAsync<T>(string channel, Func<T, Task> callback)
    {
        var sub = _connectionMultiplexer.GetSubscriber();
        (await sub.SubscribeAsync(channel)).OnMessage(async channelMessage =>
        {
            var value = channelMessage.Message;

            if (!value.IsNullOrEmpty)
            {
                var o = JsonSerializer.Deserialize<T>(value!);

                if (o != null)
                {
                    await callback(o);
                }
            }
        });
    }

    // public async Task PopAsync<T>(string queue, Action<T> callback)
    // {
    // }
    //
    // public async Task PushAsync<T>(string queue, Action<T> callback)
    // {
    // }
}