using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    // GET api/users/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingleAsync(int id)
    {
        var user = await userRepository.GetSingleAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(dto);
    }

    // GET api/users
    // GET api/users?userName=Luc
    [HttpGet]
    public ActionResult GetMany([FromQuery] string? userName)
    {
        var users = userRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            users = users.Where(user =>
                user.UserName.Contains(
                    userName,
                    StringComparison.OrdinalIgnoreCase));
        }

        var dtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        }).ToList();

        return Ok(dtos);
    }

    // POST api/users
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateAsync(
        [FromBody] CreateUserDto request)
    {
        var user = new User
        {
            UserName = request.UserName,
            Password = request.Password
        };

        var createdUser = await userRepository.AddAsync(user);

        var dto = new UserDto
        {
            Id = createdUser.Id,
            UserName = createdUser.UserName
        };

        return Ok(dto);
    }

    // PUT api/users/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAsync(int id, User user)
    {
        var existingUser = await userRepository.GetSingleAsync(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        user.Id = id;

        await userRepository.UpdateAsync(user);

        return NoContent();
    }

    // DELETE api/users/1
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        var existingUser = await userRepository.GetSingleAsync(id);

        if (existingUser == null)
        {
            return NotFound();
        }

        await userRepository.DeleteAsync(id);

        return NoContent();
    }
}