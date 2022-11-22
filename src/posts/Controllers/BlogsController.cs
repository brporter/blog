using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using posts.Models;
using posts.Services;

namespace posts.Controllers;

[Route("[controller]")]
[ApiController]
public class BlogsController
{
    readonly ILogger<BlogsController> _logger;
    readonly IBlogRepository _repository;

    public BlogsController(ILogger<BlogsController> logger, IBlogRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [Route("{slug}")]
    public async Task<Blog> GetAsync(string slug)
        => await _repository.GetBlogAsync(slug);

}
