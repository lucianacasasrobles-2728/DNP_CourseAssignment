using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();

        bool usernameExists =
            userRepository.GetMany().Any(u => u.UserName == username);

        if (usernameExists)
        {
            Console.WriteLine("Username is already taken.");
            return;
        }

        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        User user = new User
        {
            UserName = username,
            Password = password
        };

        User createdUser = await userRepository.AddAsync(user);

        Console.WriteLine($"User created with ID: {createdUser.Id}");
    }
}