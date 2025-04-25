using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.DTOs.UserDTOs;

public class UpdateUserDto
{
    public int Id { get; set; }

    [StringLength(100, MinimumLength = 3)]
    public string? Name { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    [MinLength(6)]
    public string? Password { get; init; }

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; init; }

    [Phone]
    public string? PhoneNumber { get; init; }

    [StringLength(200)]
    public string? Address { get; init; }
}
