using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string json = File.ReadAllTextAsync(filePath).Result;

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;

        comments.Add(comment);

        json = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, json);

        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        Comment? existingComment =
            comments.SingleOrDefault(c => c.Id == comment.Id);

        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        json = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        Comment? commentToRemove =
            comments.SingleOrDefault(c => c.Id == id);

        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{id}' not found");
        }

        comments.Remove(commentToRemove);

        json = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<Comment?> GetSingleAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        return comments.SingleOrDefault(c => c.Id == id);
    }

    public IQueryable<Comment> GetMany()
    {
        string json = File.ReadAllText(filePath);

        List<Comment> comments =
            JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        return comments.AsQueryable();
    }
}