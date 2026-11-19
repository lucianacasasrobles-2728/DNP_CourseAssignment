namespace BlazorApp.Auth;

public class JwtTokenProvider
{
    public string? Token { get; private set; }

    public void SetToken(string token)
    {
        Token = token;
    }

    public void ClearToken()
    {
        Token = null;
    }
}