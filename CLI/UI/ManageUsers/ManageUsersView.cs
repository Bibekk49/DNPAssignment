using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI.MangePosts;

public class ManageUsersView(IUserRepository userRepository)
{
    private readonly CreateUserView _createUserView = new(userRepository);
    private readonly ListUsersView _listUsersView = new(userRepository);

    public async Task ShowMenuAsync()
    {
        var back = false;

        while (!back)
        {
            Console.WriteLine("\nManage Users Menu:");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Delete User");
            Console.WriteLine("0. Back");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await _createUserView.ShowCreateUserViewAsync();
                    break;
                case "2":
                    await _listUsersView.ShowListUsersViewAsync();
                    break;
                case "3":
                    await UpdateUserAsync();
                    break;
                case "4":
                    await DeleteUserAsync();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }

    private async Task DeleteUserAsync()
    {
        Console.Write("\nEnter user ID to delete: ");
        int userId = Convert.ToInt32(Console.ReadLine());

        await userRepository.DeleteAsync(userId);
        Console.WriteLine($"User with id {userId} is deleted");
    }

    private async Task UpdateUserAsync()
    {
        Console.Write("\nEnter user ID to update: ");
        int userId = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter name: ");
        string? name = Console.ReadLine();

        Console.Write("Enter password: ");
        string? password = Console.ReadLine();
        userRepository.UpdateAsync(new User { Id = userId, Name = name, Password = password });
        Console.WriteLine($"User with id {userId} is updated");
    }
}