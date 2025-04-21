namespace Backend.DTOs.UserDTOs;

public record class UserDTO
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Role { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Address { get; init; }
}