using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs.UserDTOs;

public class UserSignupDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string FullName { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    [MinLength(6)]
    public required string Password { get; init; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public required string ConfirmPassword { get; init; }

    [Phone]
    public string? PhoneNumber { get; init; }

    public string? Role { get; init; } = "Tourist";

    [StringLength(200)]
    public string? Address { get; init; }
}
