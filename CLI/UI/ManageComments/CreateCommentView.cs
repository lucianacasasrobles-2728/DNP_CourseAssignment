using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CreateCommentView(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter comment: ");
        string? body = Console.ReadLine();

        Console.Write("Enter user ID: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user ID. Please enter a number.");
            return;
        }

        Console.Write("Enter post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID. Please enter a number.");
            return;
        }

        User? user = await userRepository.GetSingleAsync(userId);
        Post? post = await postRepository.GetSingleAsync(postId);

        if (user == null)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        if (post == null)
        {
            Console.WriteLine("Post does not exist.");
            return;
        }

        Comment comment = new Comment
        {
            Body = body,
            UserId = userId,
            PostId = postId
        };

        Comment createdComment =
            await commentRepository.AddAsync(comment);

        Console.WriteLine($"Comment created with ID: {createdComment.Id}");
    }
}