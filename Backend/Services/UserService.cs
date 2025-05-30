using System;
using System.Linq;
using System.Security.Cryptography;
using Backend.Data;
using Backend.DTOs;
using Backend.DTOs.UserDTOs;
using Backend.IServices;
using Backend.Models;
using Backend.WebSockets;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Backend.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public notificationSocket _nws;
    public Notificationservices _notificationservices;

    public UserService(IUnitOfWork unitOfWork, notificationSocket nws, Notificationservices notificationservices)
    {
        _nws = nws;
        _notificationservices = notificationservices;
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

        if(signupDto.Role == null)
            signupDto.Role = "Tourist";

        if (signupDto.Role != null && signupDto.Role != "Tourist" && signupDto.Role != "Agency")
            return (false, "Invalid role. Only 'Tourist' and 'Agency' are allowed.");

        if(signupDto.Role == "Agency")
           signupDto.isApproved = false;
        else
            signupDto.isApproved = true;
        var userEntity = new User
        {
            Name = signupDto.FullName,
            Email = signupDto.Email,
            PhoneNumber = signupDto.PhoneNumber,
            Address = signupDto.Address,
            Role = signupDto.Role ?? "Tourist",
            IsApproved = signupDto.isApproved,
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

        user.Name = userDto.Name ?? user.Name;
        user.Email = userDto.Email ?? user.Email;
        user.PhoneNumber = userDto.PhoneNumber ?? user.PhoneNumber;
        user.Address = userDto.Address ?? user.Address;

        await _unitOfWork.CompleteAsync();
        return (true, "User updated successfully.");
    }

    public (IEnumerable<searchResDTO> Users, int TotalCount) SearchUsersByQuery(string? q, int start, int len, bool tourist, bool agency, bool admin, bool isApproved)
    {
        var query = _unitOfWork.User.GetAllAsync().Result.AsQueryable();
        
        query = query.Where(u => u.IsApproved== isApproved);
        
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(u => u.Name.Contains(q) || u.Email.Contains(q));

        if (admin && !tourist && !agency)
            query = query.Where(u => u.Role == "Admin");
        else if (tourist && !agency && !admin)
            query = query.Where(u => u.Role == "Tourist");
        else if (!tourist && agency && !admin)
            query = query.Where(u => u.Role == "Agency");
        else if (tourist && agency && !admin)
            query = query.Where(u => u.Role == "Tourist" || u.Role == "Agency");
        else if (tourist && !agency && admin)
            query = query.Where(u => u.Role == "Tourist" || u.Role == "Admin");
        else if (!tourist && agency && admin)
            query = query.Where(u => u.Role == "Agency" || u.Role == "Admin");
        else if (tourist && agency && admin)
            query = query.Where(u => u.Role == "Tourist" || u.Role == "Agency" || u.Role == "Admin");
        else if (!tourist && !agency && !admin)
            query = query.Where(u => u.Role == "Tourist");

        int totalCount = query.Count();

        var users = query
            .Skip(start)
            .Take(len)
            .Select(u => new searchResDTO
            {
                Id = u.Id,
                Name = u.Name,
                Role = u.Role
            })
            .ToList();

        return (users, totalCount);
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
        Console.WriteLine("hashed password"+hashed);
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

    public async Task<bool> ApproveUserAsync(int id)
    {
        var user = _unitOfWork.User.GetByIdAsync(id).Result;
        if (user == null) return false;

        user.IsApproved = true;
        _unitOfWork.User.Update(user);

        var message = new {
            content = "Your account has been approved."
        };
        var messageJson = System.Text.Json.JsonSerializer.Serialize(message);
        _nws.SendMessageToUserAsync(user.Id, messageJson);

        var notification = new NotificationDto
        {
            SenderId = 1,
            ReceiverId = user.Id,
            Context = "Your account has been approved.",
        };
        await _notificationservices.SendNotificationAsync(notification,new List<int> { user.Id },false);
        await _unitOfWork.CompleteAsync();
        return true;
    }


    // public object SearchUsers(int start, int len, bool? tourist, bool? agency)
    // {
    //     var usersQuery = _unitOfWork.User.GetAllAsync().Result.AsQueryable();

    //     if (tourist.HasValue && tourist.Value)
    //     {
    //         usersQuery = usersQuery.Where(u => u.Role == "Tourist");
    //     }

    //     if (agency.HasValue && agency.Value)
    //     {
    //         usersQuery = usersQuery.Where(u => u.Role == "Agency");
    //     }

    //     var paginatedUsers = usersQuery
    //         .Skip(start)
    //         .Take(len)
    //         .Select(u => new searchResDTO
    //         {
    //             Id = u.Id,
    //             Name = u.Name,
    //             Role = u.Role
    //         })
    //         .ToList();

    //     return paginatedUsers;
    // }
}
