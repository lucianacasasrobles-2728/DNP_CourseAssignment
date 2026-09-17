using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public SinglePostView(
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("Enter post ID: ");

        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid post ID. Please enter a number.");
            return;
        }

        Post? post = await postRepository.GetSingleAsync(postId);

        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine("Comments:");

        IQueryable<Comment> comments = commentRepository.GetMany();

        foreach (Comment comment in comments.Where(c => c.PostId == post.Id))
        {
            Console.WriteLine($"- {comment.Body}");
        }
    }
}