using System.Security.Claims;
using Backend.Data;
using Backend.DTOs.UserDTOs;
using Backend.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/user
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any()) return NotFound("No users found.");
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int? id)
        {
            // Get the user ID from the token
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not found in token.");

            // If id is null, use the ID from the token
            if (id == null) id = int.Parse(userIdClaim);

            var result = await _userService.GetUserByIdAsync((int)id);
            if (!result.Success) return NotFound(result.Message);

            var retuser = result.User;
            if (retuser == null) return NotFound("User not found.");
            // Allow access if the user is the same as in the token or has the "Agency" role
            if (id.ToString() == userIdClaim || retuser?.Role == "Agency")
            {
                // return userDTO
                var user = new UserDTO
                {
                    Id = retuser.Id,
                   Name = retuser.Name,
                    Email = retuser.Email,
                    PhoneNumber = retuser.PhoneNumber,
                    Role = retuser.Role,
                };
                return Ok(user);
            }

            // Check if the user has the "Admin" or "Agency" role
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roleClaim != "Admin" && roleClaim != "Agency")
            {
                return Unauthorized("You are not authorized to view this user.");
            }
            var userDto = new UserDTO
                {
                    Id = retuser.Id,
                   Name = retuser.Name,
                    Email = retuser.Email,
                    PhoneNumber = retuser.PhoneNumber,
                    Role = retuser.Role,
                };
                return Ok(userDto);
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            
            return success ? NoContent() : NotFound("User not found.");
        }
        //update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userUpdateDto,int id)
        {
            // Check if the user reqested to update is the same as the one in the token
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized("User not found in token.");

            var result = await _userService.UpdateUserAsync(userUpdateDto, id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Success);
        }
        // approve user
        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var result = await _userService.ApproveUserAsync(id);
            if (!result) return BadRequest("User not found.");
            return Ok(result);
        }
    }
}
