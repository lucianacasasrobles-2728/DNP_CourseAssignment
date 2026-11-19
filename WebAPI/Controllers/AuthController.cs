using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly IConfiguration configuration;

    public AuthController(
        IUserRepository userRepository,
        IConfiguration configuration)
    {
        this.userRepository = userRepository;
        this.configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        User? user = userRepository.GetMany()
            .FirstOrDefault(u => u.UserName == request.Username);

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Invalid username or password");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "DNPWebAPI",
            audience: "DNPBlazorApp",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            userId = user.Id,
            userName = user.UserName
        });
    }
}