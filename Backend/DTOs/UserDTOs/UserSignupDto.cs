using Backend.Models;

namespace Backend.DTOs.UserDTOs;

public class UserSignupDto
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
    public string? PhoneNumber { get; init; } 
    public string? Role { get; init; } = "User"; // Default role is "User"  
    public string? Address { get; init; } // Add this line
}
