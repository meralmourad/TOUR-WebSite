namespace Backend.DTOs.UserDTOs;

public class UserLoginDTO
{
    public required string Email { get; set; } 
    public required string Password { get; set; }
    // mappers
    public static UserLoginDTO FromUserDto(UserLoginDTO userLoginDto)
    {
        return new UserLoginDTO
        {
            Email = userLoginDto.Email,
            Password = userLoginDto.Password
        };
    }
}
