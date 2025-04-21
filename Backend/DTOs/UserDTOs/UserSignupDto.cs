using Backend.Models;

namespace Backend.DTOs.UserDTOs;

public class UserSignupDto
{
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
    public int? AddressId { get; init; } // Add this line

    // mappers
    public static UserSignupDto FromUserDto(UserSignupDto userSignupDto)
    {
        return new UserSignupDto
        {
            FullName = userSignupDto.FullName,
            Email = userSignupDto.Email,
            Password = userSignupDto.Password,
            ConfirmPassword = userSignupDto.ConfirmPassword
        };
    }
    public static User ToDb(UserSignupDto userSignupDto)
    {
        return new User
        {
            Name = userSignupDto.FullName,
            Email = userSignupDto.Email,
            Password = userSignupDto.Password,
        };
    }

}
