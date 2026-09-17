using CLI.UI.ManageComments;
using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        ManageUsersView manageUsersView =
            new ManageUsersView(userRepository);

        ManagePostsView managePostsView =
            new ManagePostsView(
                postRepository,
                commentRepository,
                userRepository);

        CreateCommentView createCommentView =
            new CreateCommentView(
                commentRepository,
                userRepository,
                postRepository);

        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("--- Main Menu ---");
            Console.WriteLine("1. Manage users");
            Console.WriteLine("2. Manage posts");
            Console.WriteLine("3. Add comment");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await manageUsersView.ShowAsync();
                    break;

                case "2":
                    await managePostsView.ShowAsync();
                    break;

                case "3":
                    await createCommentView.ShowAsync();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}