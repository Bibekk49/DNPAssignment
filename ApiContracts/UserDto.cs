namespace ApiContracts;

public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Password { get; set; }
}
