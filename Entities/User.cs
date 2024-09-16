using System.Diagnostics.Contracts;

namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public User(string name)
    {
        Contract.Requires(!string.IsNullOrWhiteSpace(name));
        Name = name;
    }

    public User()
    {
        throw new NotImplementedException();
    }
}