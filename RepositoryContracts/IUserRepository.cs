using Entities;

namespace RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> GetSingleAsync(int id);
        IQueryable<User> GetMany();
        Task<bool> UsernameExistsAsync(string userDtoName);
        Task<User?> AuthenticateUserAsync(string username, string password);
    }
}