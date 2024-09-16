using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public Task ShowCreateUserViewAsync()
    {
        Console.Write("Enter name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter password: ");
        string? password = Console.ReadLine();
        
        if (name is null || password is null)
        {
            return Task.CompletedTask;
        }
        var user = new User { Name = name, Password = password };
        Console.WriteLine($"New User with id {user.Id} is created ");
        return Task.CompletedTask;
    }
}