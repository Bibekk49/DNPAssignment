namespace ApiContracts;

public class UpdateUserDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
}