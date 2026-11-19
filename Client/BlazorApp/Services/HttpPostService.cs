using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ApiContracts;
using BlazorApp.Auth;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;
    private readonly JwtTokenProvider tokenProvider;

    public HttpPostService(
        HttpClient client,
        JwtTokenProvider tokenProvider)
    {
        this.client = client;
        this.tokenProvider = tokenProvider;
    }

    public async Task CreatePostAsync(CreatePostDto request)
    {
        using HttpRequestMessage message = new(
            HttpMethod.Post, "api/posts");

        message.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer", tokenProvider.Token);

        message.Content = JsonContent.Create(request);

        HttpResponseMessage response =
            await client.SendAsync(message);

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
            return null;

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PostDto>();
    }
}