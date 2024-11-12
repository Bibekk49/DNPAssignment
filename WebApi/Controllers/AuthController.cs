using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

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
        var user = await _userRepository.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);
        if (user == null) return Unauthorized("Invalid username or password");

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,

        };

        return Ok(userDto);
    }
}
