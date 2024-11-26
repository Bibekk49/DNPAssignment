using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

public class LoginRequest
{
    public string Username { get; set; }  // Use Username here
    public string Password { get; set; }
}

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _userRepository.AuthenticateUserAsync(loginRequest.Username, loginRequest.Password);
        if (user == null) return Unauthorized("Invalid username or password");

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Password = user.Password
        };

        return Ok(userDto);
    }
}