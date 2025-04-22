namespace Backend.DTOs.UserDTOs;

public class UserDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}