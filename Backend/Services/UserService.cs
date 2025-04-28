using System;
using System.Security.Cryptography;
using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.UserDTOs;
using Backend.IServices;
using Backend.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Backend.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.User.GetAllAsync();
        return users.Select(u => new UserDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Address = u.Address?.ToString(),
            Role = u.Role
        }).ToList();
    }

    public async Task<(bool Success, string Message)> RegisterUserAsync(UserSignupDto signupDto)
    {
        if (signupDto.Password != signupDto.ConfirmPassword)
            return (false, "Passwords do not match.");

        var existing = await _unitOfWork.User.GetUserByEmailAsync(signupDto.Email);
        if (existing != null)
            return (false, "User already exists.");

        if (signupDto.Role != null && signupDto.Role != "Tourist" && signupDto.Role != "Agency")
            return (false, "Invalid role. Only 'Tourist' and 'Agency' are allowed.");

        var userEntity = new User
        {
            Name = signupDto.FullName,
            Email = signupDto.Email,
            PhoneNumber = signupDto.PhoneNumber,
            Address = signupDto.Address,
            Role = signupDto.Role ?? "Tourist",
            Password = HashPassword(signupDto.Password)
        };

        await _unitOfWork.User.AddAsync(userEntity);
        await _unitOfWork.CompleteAsync();
        return (true, "User created successfully.");
    }

    public async Task<(bool Success, UserDTO? User, string Message)> LoginAsync(UserLoginDTO loginDto)
    {
        var user = await _unitOfWork.User.GetUserByEmailAsync(loginDto.Email);
        if (user == null)
            return (false, null, "User not found.");

        if (!VerifyPassword(loginDto.Password, user.Password))
            return (false, null, "Invalid password.");

        var dto = new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address?.ToString(),
            Role = user.Role
        };

        return (true, dto, "Login successful.");
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.User.GetByIdAsync(id);
        if (user == null) return false;

        _unitOfWork.User.Delete(user);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<(bool Success, string Message)> UpdateUserAsync(UpdateUserDto userDto, int id)
    {
        var user = await _unitOfWork.User.GetByIdAsync(id);
        if (user == null)
            return (false, "User not found.");

        if (userDto.Password != null)
        {
            if (userDto.Password != userDto.ConfirmPassword)
                return (false, "Passwords do not match.");
            user.Password = HashPassword(userDto.Password);
        }

        user.Name = userDto.Name ?? user.Name;
        user.Email = userDto.Email ?? user.Email;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.Address = userDto.Address ?? user.Address;

        await _unitOfWork.CompleteAsync();
        return (true, "User updated successfully.");
    }

    public IEnumerable<searchResDTO> SearchUsersByQuery(string? query, int start, int len, bool? tourist, bool? agency)
    {
        var usersQuery = _unitOfWork.User.GetAllAsync().Result.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            usersQuery = usersQuery.Where(u => u.Name.Contains(query) || u.Email.Contains(query));
        }

        if (tourist.HasValue && tourist.Value)
        {
            usersQuery = usersQuery.Where(u => u.Role == "Tourist");
        }

        if (agency.HasValue && agency.Value)
        {
            usersQuery = usersQuery.Where(u => u.Role == "Agency");
        }

        return usersQuery
            .Skip(start)
            .Take(len)
            .Select(u => new searchResDTO
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role
            })
            .ToList();
    }

    public async Task<(bool Success, UserDTO? User, string Message)> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.User.GetByIdAsync(id);
        if (user == null)
            return (false, null, "User not found.");

        var userDto = new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address?.ToString(),
            Role = user.Role
        };

        return (true, userDto, "User retrieved successfully.");
    }

    private string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));
        return $"{Convert.ToBase64String(salt)}.{hashed}";
    }

    private bool VerifyPassword(string input, string stored)
    {
        var parts = stored.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            input,
            salt,
            KeyDerivationPrf.HMACSHA256,
            100000,
            256 / 8));
        return hashed == parts[1];
    }

    public async Task<IEnumerable<searchResDTO>> SearchUsersAsync(string query)
    {
        var users = await _unitOfWork.User.GetAllAsync();
        var filteredUsers = users
            .Where(u => u.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || 
                        u.Email.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Select(u => new searchResDTO
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role
            })
            .ToList();

        return filteredUsers;
    }

    public object SearchUsers(int start, int len, bool? tourist, bool? agency)
    {
        var usersQuery = _unitOfWork.User.GetAllAsync().Result.AsQueryable();

        if (tourist.HasValue && tourist.Value)
        {
            usersQuery = usersQuery.Where(u => u.Role == "Tourist");
        }

        if (agency.HasValue && agency.Value)
        {
            usersQuery = usersQuery.Where(u => u.Role == "Agency");
        }

        var paginatedUsers = usersQuery
            .Skip(start)
            .Take(len)
            .Select(u => new searchResDTO
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role
            })
            .ToList();

        return paginatedUsers;
    }
}
