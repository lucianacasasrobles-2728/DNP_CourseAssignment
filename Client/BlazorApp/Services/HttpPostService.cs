using System.Net;
using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task CreatePostAsync(CreatePostDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("api/posts", request);

        response.EnsureSuccessStatusCode();
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        List<PostDto>? posts =
            await client.GetFromJsonAsync<List<PostDto>>("api/posts");

        return posts ?? new List<PostDto>();
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        HttpResponseMessage response =
            await client.GetAsync($"api/posts/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PostDto>();
    }
}