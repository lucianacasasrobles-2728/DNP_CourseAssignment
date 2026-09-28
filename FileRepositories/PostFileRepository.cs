using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;

        posts.Add(post);

        json = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, json);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        Post? existingPost =
            posts.SingleOrDefault(p => p.Id == post.Id);

        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        json = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        Post? postToRemove =
            posts.SingleOrDefault(p => p.Id == id);

        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);

        json = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<Post?> GetSingleAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        return posts.SingleOrDefault(p => p.Id == id);
    }

    public IQueryable<Post> GetMany()
    {
        string json = File.ReadAllTextAsync(filePath).Result;

        List<Post> posts =
            JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        return posts.AsQueryable();
    }
}