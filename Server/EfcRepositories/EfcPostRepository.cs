using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppDbContext context;

    public EfcPostRepository(AppDbContext context)
    {
        this.context = context;
    }

    // CREATE - Add a new post
    public async Task<Post> AddAsync(Post post)
    {
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();

        return post;
    }

    // UPDATE - Update an existing post
    public async Task UpdateAsync(Post post)
    {
        Post? existingPost = await context.Posts.FindAsync(post.Id);

        if (existingPost == null)
        {
            throw new KeyNotFoundException(
                $"Post with ID {post.Id} not found.");
        }

        existingPost.Title = post.Title;
        existingPost.Body = post.Body;
        existingPost.UserId = post.UserId;

        await context.SaveChangesAsync();
    }

    // DELETE - Delete a post by ID
    public async Task DeleteAsync(int id)
    {
        Post? post = await context.Posts.FindAsync(id);

        if (post == null)
        {
            throw new KeyNotFoundException(
                $"Post with ID {id} not found.");
        }

        context.Posts.Remove(post);
        await context.SaveChangesAsync();
    }

    // READ - Get a single post by ID
    public async Task<Post?> GetSingleAsync(int id)
    {
        return await context.Posts.FindAsync(id);
    }

    // READ - Get all posts
    public IQueryable<Post> GetMany()
    {
        return context.Posts.AsNoTracking();
    }
}