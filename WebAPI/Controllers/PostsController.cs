
using System.Security.Claims;
using Entities;
using Microsoft.AspNetCore.Authorization;
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
            return NotFound();

        return Ok(post);
    }

    // GET api/posts
    [HttpGet]
    public ActionResult GetMany(
        [FromQuery] int? userId,
        [FromQuery] string? title)
    {
        var posts = postRepository.GetMany();

        if (userId != null)
            posts = posts.Where(p => p.UserId == userId);

        if (!string.IsNullOrWhiteSpace(title))
            posts = posts.Where(p =>
                p.Title.ToLower().Contains(title.ToLower()));

        return Ok(posts.ToList());
    }

    // POST api/posts
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateAsync(Post post)
    {
        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        post.UserId = userId;

        var createdPost = await postRepository.AddAsync(post);

        return Ok(createdPost);
    }

    // PUT api/posts/1
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, Post post)
    {
        var existingPost = await postRepository.GetSingleAsync(id);

        if (existingPost == null)
            return NotFound();

        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (existingPost.UserId != userId)
            return Forbid();

        post.Id = id;
        post.UserId = userId;

        await postRepository.UpdateAsync(post);

        return NoContent();
    }

    // DELETE api/posts/1
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var existingPost = await postRepository.GetSingleAsync(id);

        if (existingPost == null)
            return NotFound();

        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (existingPost.UserId != userId)
            return Forbid();

        await postRepository.DeleteAsync(id);

        return NoContent();
    }
}
