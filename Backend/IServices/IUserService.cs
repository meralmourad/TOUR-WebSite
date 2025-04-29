using System;
using Backend.DTOs;
using Backend.DTOs.UserDTOs;
using Backend.Models;

namespace Backend.IServices;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<(bool Success, string Message)> RegisterUserAsync(UserSignupDto signupDto);
    Task<(bool Success, UserDTO? User, string Message)> LoginAsync(UserLoginDTO loginDto);
    Task<(bool Success, string Message)> UpdateUserAsync(UpdateUserDto userDto,int id);
    Task<bool> DeleteUserAsync(int id);
    Task<IEnumerable<searchResDTO>> SearchUsersAsync(string query);
    object SearchUsers(int start, int len, bool? tourist, bool? agency);
    IEnumerable<searchResDTO> SearchUsersByQuery(string? query, int start, int len, bool tourist, bool agency,bool admin);
    Task<(bool Success, UserDTO? User, string Message)> GetUserByIdAsync(int id);
}
