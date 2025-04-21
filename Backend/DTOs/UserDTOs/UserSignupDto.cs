namespace Backend.DTOs.UserDTOs;

public record class UserSignupDto
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}
