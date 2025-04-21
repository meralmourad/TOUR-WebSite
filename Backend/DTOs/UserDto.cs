namespace Backend.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    // mappers
    public static UserDto FromUserDto(UserDto userDto)
    {
        return new UserDto
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
        };
    }
}
