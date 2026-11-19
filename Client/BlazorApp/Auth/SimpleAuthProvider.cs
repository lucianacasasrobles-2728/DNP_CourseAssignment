
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly JwtTokenProvider tokenProvider;
    private ClaimsPrincipal currentClaimsPrincipal = new();

    public SimpleAuthProvider(
        HttpClient httpClient,
        JwtTokenProvider tokenProvider)
    {
        this.httpClient = httpClient;
        this.tokenProvider = tokenProvider;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginRequest
            {
                Username = userName,
                Password = password
            });

        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        using JsonDocument document = JsonDocument.Parse(content);

        string token = document.RootElement
            .GetProperty("token").GetString()!;

        int userId = document.RootElement
            .GetProperty("userId").GetInt32();

        string returnedUserName = document.RootElement
            .GetProperty("userName").GetString()!;

        tokenProvider.SetToken(token);

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, returnedUserName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim("Id", userId.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "jwt");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(currentClaimsPrincipal)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(
            new AuthenticationState(currentClaimsPrincipal));
    }

    public void Logout()
    {
        tokenProvider.ClearToken();

        currentClaimsPrincipal = new ClaimsPrincipal();

        NotifyAuthenticationStateChanged(
            Task.FromResult(
                new AuthenticationState(currentClaimsPrincipal)));
    }
}
