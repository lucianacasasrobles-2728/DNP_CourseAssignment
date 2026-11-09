using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task CreateCommentAsync(CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsAsync();
    Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId);
}