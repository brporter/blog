using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using posts.Models;
using posts.Services;

namespace posts.Controllers;

[Route("[controller]")]
[ApiController]
public class PostsController 
    : ControllerBase
{
    readonly ILogger<PostsController> _logger;
    readonly IPostRepository _postRepository;

    public PostsController(ILogger<PostsController> logger, IPostRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _postRepository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [Route("{handle}")]
    public async Task<IEnumerable<Post>> GetAsync(string handle, int page = 1)
        => await _postRepository.GetPostsAsync(handle, page);
}
