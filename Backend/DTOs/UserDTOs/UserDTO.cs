using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.UserDTOs;

public class UserDTO
{
    public required int Id { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; init; }

    [Required]
    [EmailAddress]
    public required string Email { get; init; }

    [Required]
    public required string Role { get; init; }

    [Phone]
    public string? PhoneNumber { get; init; }

    [StringLength(200)]
    public string? Address { get; init; }
}