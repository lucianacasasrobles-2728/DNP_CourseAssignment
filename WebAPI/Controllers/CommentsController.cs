
using System.Security.Claims;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    // GET api/comments/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetSingleAsync(int id)
    {
        var comment = await commentRepository.GetSingleAsync(id);

        if (comment == null)
            return NotFound();

        return Ok(comment);
    }

    // GET api/comments
    [HttpGet]
    public ActionResult GetMany(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        var comments = commentRepository.GetMany();

        if (userId != null)
            comments = comments.Where(c => c.UserId == userId);

        if (postId != null)
            comments = comments.Where(c => c.PostId == postId);

        return Ok(comments.ToList());
    }

    // POST api/comments
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateAsync(Comment comment)
    {
        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        comment.UserId = userId;

        var createdComment = await commentRepository.AddAsync(comment);

        return Ok(createdComment);
    }

    // PUT api/comments/1
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, Comment comment)
    {
        var existingComment =
            await commentRepository.GetSingleAsync(id);

        if (existingComment == null)
            return NotFound();

        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (existingComment.UserId != userId)
            return Forbid();

        comment.Id = id;
        comment.UserId = userId;
        comment.PostId = existingComment.PostId;

        await commentRepository.UpdateAsync(comment);

        return NoContent();
    }

    // DELETE api/comments/1
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var existingComment =
            await commentRepository.GetSingleAsync(id);

        if (existingComment == null)
            return NotFound();

        int userId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (existingComment.UserId != userId)
            return Forbid();

        await commentRepository.DeleteAsync(id);

        return NoContent();
    }
}
