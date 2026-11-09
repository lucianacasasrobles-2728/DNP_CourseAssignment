using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task CreatePostAsync(CreatePostDto request);
    Task<List<PostDto>> GetPostsAsync();
    Task<PostDto?> GetPostByIdAsync(int id);
}