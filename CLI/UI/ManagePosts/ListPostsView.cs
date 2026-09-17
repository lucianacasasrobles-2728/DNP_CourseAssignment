using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void Show()
    {
        IQueryable<Post> posts = postRepository.GetMany();

        Console.WriteLine("Posts:");

        foreach (Post post in posts)
        {
            Console.WriteLine($"Title: {post.Title} - ID: {post.Id}");
        }
    }
}