using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task CreateCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("api/comments", request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<CommentDto>> GetCommentsAsync()
    {
        List<CommentDto>? comments =
            await client.GetFromJsonAsync<List<CommentDto>>("api/comments");

        return comments ?? new List<CommentDto>();
    }

    public async Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId)
    {
        List<CommentDto>? comments =
            await client.GetFromJsonAsync<List<CommentDto>>(
                $"api/comments?postId={postId}");

        return comments ?? new List<CommentDto>();
    }
}