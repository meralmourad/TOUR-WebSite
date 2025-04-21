namespace Backend.DTOs.UserDTOs;
public record class UpdateUserDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
