using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;

    public PostsController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    // GET api/posts/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetSingleAsync(int id)
    {
        var post = await postRepository.GetSingleAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        return Ok(post);
    }

    // GET api/posts
    // GET api/posts?userId=4
    // GET api/posts?title=first
    // GET api/posts?userId=4&title=first
    [HttpGet]
    public ActionResult GetMany(
        [FromQuery] int? userId,
        [FromQuery] string? title)
    {
        var posts = postRepository.GetMany();

        if (userId != null)
        {
            posts = posts.Where(post => post.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(post =>
                post.Title.Contains(
                    title,
                    StringComparison.OrdinalIgnoreCase));
        }

        return Ok(posts.ToList());
    }

    // POST api/posts
    [HttpPost]
    public async Task<ActionResult> CreateAsync(Post post)
    {
        var createdPost = await postRepository.AddAsync(post);

        return Ok(createdPost);
    }

    // PUT api/posts/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, Post post)
    {
        var existingPost = await postRepository.GetSingleAsync(id);

        if (existingPost == null)
        {
            return NotFound();
        }

        post.Id = id;

        await postRepository.UpdateAsync(post);

        return NoContent();
    }

    // DELETE api/posts/1
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var existingPost = await postRepository.GetSingleAsync(id);

        if (existingPost == null)
        {
            return NotFound();
        }

        await postRepository.DeleteAsync(id);

        return NoContent();
    }
}