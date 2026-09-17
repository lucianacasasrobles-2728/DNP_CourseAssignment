using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter title: ");
        string? title = Console.ReadLine();

        Console.Write("Enter body: ");
        string? body = Console.ReadLine();

        Console.Write("Enter user ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID. Please enter a number.");
            return;
        }

        User? user = await userRepository.GetSingleAsync(userId);

        if (user == null)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post createdPost = await postRepository.AddAsync(post);

        Console.WriteLine($"Post created with ID: {createdPost.Id}");
    }
}