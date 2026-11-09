using System.Net;
using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response =
            await client.PostAsJsonAsync("api/users", request);

        response.EnsureSuccessStatusCode();

        UserDto? user =
            await response.Content.ReadFromJsonAsync<UserDto>();

        return user ?? throw new Exception("No user returned from API.");
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        List<UserDto>? users =
            await client.GetFromJsonAsync<List<UserDto>>("api/users");

        return users ?? new List<UserDto>();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        HttpResponseMessage response =
            await client.GetAsync($"api/users/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}