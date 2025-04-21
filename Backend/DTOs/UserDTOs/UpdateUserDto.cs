using Backend.Models;

namespace Backend.DTOs.UserDTOs;
public class UpdateUserDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? ConfirmPassword { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    //mappers
    public static UpdateUserDto FromUserDto(UserDTO userDto)
    {
        return new UpdateUserDto
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Address = userDto.Address
        };
    }
    public static User ToDb(UpdateUserDto userDto)
    {
        return new User
        {
            Id = userDto.Id,
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
        };
    }
}
