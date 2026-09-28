using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        string json = File.ReadAllTextAsync(filePath).Result;
        
        List<User> users =
            JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;

        users.Add(user);

        json = JsonSerializer.Serialize(users);

        await File.WriteAllTextAsync(filePath, json);

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<User> users =
            JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        User? existingUser =
            users.SingleOrDefault(u => u.Id == user.Id);

        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }

        users.Remove(existingUser);
        users.Add(user);

        json = JsonSerializer.Serialize(users);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<User> users =
            JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        User? userToRemove =
            users.SingleOrDefault(u => u.Id == id);

        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        users.Remove(userToRemove);

        json = JsonSerializer.Serialize(users);

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<User?> GetSingleAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);

        List<User> users =
            JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        return users.SingleOrDefault(u => u.Id == id);
    }

    public IQueryable<User> GetMany()
    {
        string json = File.ReadAllText(filePath);

        List<User> users =
            JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        return users.AsQueryable();
    }
}