using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using posts.Models;
using posts.Services;

namespace posts.Controllers;

[Route("[controller]")]
[ApiController]
public class BlogsController
    : ControllerBase
{
    readonly ILogger<BlogsController> _logger;
    readonly IBlogRepository _repository;
    private readonly ICache _cache;

    public BlogsController(ILogger<BlogsController> logger, IBlogRepository repository, ICache cache)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    [HttpGet]
    [Route("{slug}")]
    public async Task<IActionResult> GetAsync(string slug)
    {
        var rv = await _cache.GetValueAsync<Blog>($"blog:{slug}");

        if (!rv.HasValue)
        {
            rv = await _repository.GetBlogAsync(slug);
        }

        if (rv.HasValue)
            return Ok(rv.Value);

        return NotFound();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> PostAsync(Blog blogDetails)
    {
        var rv = await _repository.GetBlogAsync(blogDetails.Slug);

        if (rv.HasValue)
            return Conflict();

        var sanitized = blogDetails with
        {
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            BlogId = 0,
            UserId = 1,
            Enabled = true
        };
        
        await _cache.QueuePublishAsync("blogcreation", sanitized);

        return Accepted();
    }
}
