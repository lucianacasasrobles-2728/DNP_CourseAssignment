using CLI.UI;
using FileRepositories;
using RepositoryContracts;

namespace CLI;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting CLI Application...");

        IUserRepository userRepository = new UserFileRepository();
        ICommentRepository commentRepository = new CommentFileRepository();
        IPostRepository postRepository = new PostFileRepository();

        CliApp app = new CliApp(
            userRepository,
            commentRepository,
            postRepository
        );

        await app.StartAsync();
    }
}