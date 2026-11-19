using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppDbContext context;

    public EfcCommentRepository(AppDbContext context)
    {
        this.context = context;
    }

    // CREATE - Add a new comment
    public async Task<Comment> AddAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();

        return comment;
    }

    // UPDATE - Update an existing comment
    public async Task UpdateAsync(Comment comment)
    {
        Comment? existingComment =
            await context.Comments.FindAsync(comment.Id);

        if (existingComment == null)
        {
            throw new KeyNotFoundException(
                $"Comment with ID {comment.Id} not found.");
        }

        existingComment.Body = comment.Body;
        existingComment.UserId = comment.UserId;
        existingComment.PostId = comment.PostId;

        await context.SaveChangesAsync();
    }

    // DELETE - Delete a comment by ID
    public async Task DeleteAsync(int id)
    {
        Comment? comment = await context.Comments.FindAsync(id);

        if (comment == null)
        {
            throw new KeyNotFoundException(
                $"Comment with ID {id} not found.");
        }

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
    }

    // READ - Get a single comment by ID
    public async Task<Comment?> GetSingleAsync(int id)
    {
        return await context.Comments.FindAsync(id);
    }

    // READ - Get all comments
    public IQueryable<Comment> GetMany()
    {
        return context.Comments.AsNoTracking();
    }
}