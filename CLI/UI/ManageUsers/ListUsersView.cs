using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView(IUserRepository userRepository)
{
    public Task ShowListUsersViewAsync()
    {
        var queryableUser = userRepository.GetMany();

        if (!queryableUser.Any())
        {
            Console.WriteLine("There are no users");
        }

        foreach (var user in queryableUser)
        {
            Console.WriteLine("Id: " + user.Id + " Name: " + user.Name);
        }

        return Task.CompletedTask;
    }
}