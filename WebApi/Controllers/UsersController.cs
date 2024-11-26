using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            // Check if the username already exists
            bool usernameExists = await _userRepository.UsernameExistsAsync(userDto.Name);
            if (usernameExists)
            {
                return Conflict("Username already exists");
            }

            // Create and save the new user
            var user = new User
            {
                Name = userDto.Name,
                Password = userDto.Password
            };
            await _userRepository.AddAsync(user);

            // Return the created user as a UserDto
            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Password = user.Password
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IResult> GetSingleAsync(int id)
        {
            User user = await _userRepository.GetSingleAsync(id);
            return Results.Ok(new UserDto { Id = user.Id, Name = user.Name, Password = user.Password });
        }

        [HttpGet]
        public async Task<IResult> GetManyAsync([FromQuery] string? name)
        {
            IQueryable<User> users = _userRepository.GetMany();

            // Filter users by the "name" query parameter
            if (!string.IsNullOrEmpty(name))
            {
                users = users.Where(u => u.Name.ToLower().Contains(name.ToLower()));
            }

            IQueryable<UserDto> userDtos = users.Select(u => new UserDto { Id = u.Id, Name = u.Name, Password = u.Password });

            return Results.Ok(userDtos);
        }

        [HttpPut("{id:int}")]
        public async Task<IResult> UpdateAsync(int id, CreateUserDto userDto)
        {
            await _userRepository.UpdateAsync(new User { Id = id, Name = userDto.Name, Password = userDto.Password });
            return Results.Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IResult> DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
            return Results.Ok();
        }
    }
}
