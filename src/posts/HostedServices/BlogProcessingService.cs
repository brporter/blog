using posts.Models;
using posts.Services;

namespace posts.HostedServices;

public class BlogProcessingService
    : IHostedService
{
    private readonly ILogger<BlogProcessingService> _logger;
    private readonly ICache _cache;
    private readonly IBlogRepository _repository;
    private readonly CancellationTokenSource _tokenSource;
    private Task? _backgroundTask;

    public BlogProcessingService(ILogger<BlogProcessingService> logger, ICache cache, IBlogRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _tokenSource = new CancellationTokenSource();
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _cache.QueueSubscribeAsync<Blog>("blogcreation", async (blog) =>
        {
            
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}