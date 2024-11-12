using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        var usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 0;
        user.Id = maxId + 1;
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        return user;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.Any(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User?> AuthenticateUserAsync(string username, string password)
    {
        var usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        // Check if there is a user with the matching username and password
        return users.SingleOrDefault(u => 
            u.Name.Equals(username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password); // In production, use hashed passwords
    }

    public async Task UpdateAsync(User user)
    {
        var usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        var userToUpdate = users.FirstOrDefault(c => c.Id == user.Id);
        if (userToUpdate != null)
        {
            users.Remove(userToUpdate);
            users.Add(user);
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        var userToDelete = users.SingleOrDefault(c => c.Id == id);
        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, usersAsJson);
        }
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var usersAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        User? user = users.SingleOrDefault(c => c.Id == id);
        return user ?? throw new Exception("User not found");
    }

    public IQueryable<User> GetMany()
    {
        var usersAsJson = File.ReadAllTextAsync(filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable() ?? throw new Exception("No users found");
    }
}
