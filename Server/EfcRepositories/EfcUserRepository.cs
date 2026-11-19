using Entities;
using RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppDbContext context;

    public EfcUserRepository(AppDbContext context)
    {
        this.context = context;
    }

    // CREATE - Add a new user
    public async Task<User> AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return user;
    }

    // UPDATE - Update an existing user
    public async Task UpdateAsync(User user)
    {
        User? existingUser = await context.Users.FindAsync(user.Id);

        if (existingUser == null)
        {
            throw new KeyNotFoundException(
                $"User with ID {user.Id} not found.");
        }

        existingUser.UserName = user.UserName;
        existingUser.Password = user.Password;

        await context.SaveChangesAsync();
    }

    // DELETE - Delete a user by ID
    public async Task DeleteAsync(int id)
    {
        User? user = await context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException(
                $"User with ID {id} not found.");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    // READ - Get a single user by ID
    public async Task<User?> GetSingleAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    // READ - Get all users
    public IQueryable<User> GetMany()
    {
        return context.Users.AsNoTracking();
    }
}