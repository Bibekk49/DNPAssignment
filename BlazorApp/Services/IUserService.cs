using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task<UserDto> GetSingleAsync(int id);
    Task<IEnumerable<UserDto>> GetManyAsync(string? name);
    Task DeleteUserAsync(int id);
}