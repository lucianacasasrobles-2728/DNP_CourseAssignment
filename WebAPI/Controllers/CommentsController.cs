using Entities;
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
        {
            return NotFound();
        }

        return Ok(comment);
    }

    // GET api/comments
    // GET api/comments?userId=2
    // GET api/comments?postId=5
    // GET api/comments?userId=2&postId=5
    [HttpGet]
    public ActionResult GetMany(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        var comments = commentRepository.GetMany();

        if (userId != null)
        {
            comments = comments.Where(
                comment => comment.UserId == userId);
        }

        if (postId != null)
        {
            comments = comments.Where(
                comment => comment.PostId == postId);
        }

        return Ok(comments.ToList());
    }

    // POST api/comments
    [HttpPost]
    public async Task<ActionResult> CreateAsync(Comment comment)
    {
        var createdComment = await commentRepository.AddAsync(comment);

        return Ok(createdComment);
    }

    // PUT api/comments/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(
        int id,
        Comment comment)
    {
        var existingComment =
            await commentRepository.GetSingleAsync(id);

        if (existingComment == null)
        {
            return NotFound();
        }

        comment.Id = id;

        await commentRepository.UpdateAsync(comment);

        return NoContent();
    }

    // DELETE api/comments/1
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var existingComment =
            await commentRepository.GetSingleAsync(id);

        if (existingComment == null)
        {
            return NotFound();
        }

        await commentRepository.DeleteAsync(id);

        return NoContent();
    }
}