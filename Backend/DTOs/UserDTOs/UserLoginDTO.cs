namespace Backend.DTOs.UserDTOs;

public record class UserLoginDTO
{
    public required string Email { get; set; } 
    public required string Password { get; set; }
}
