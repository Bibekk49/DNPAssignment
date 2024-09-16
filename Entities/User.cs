using System.Diagnostics.Contracts;

namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public User(string username)
    {
        Contract.Requires(!string.IsNullOrWhiteSpace(username));
        Username = username;
    }

    public User()
    {
        throw new NotImplementedException();
    }
}