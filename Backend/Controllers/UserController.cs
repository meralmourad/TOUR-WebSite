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

            // Allow access if the user is the same as in the token or has the "Agency" role
            if (id.ToString() == userIdClaim || retuser?.Role == "Agency")
            {
                return Ok(result.User);
            }

            // Check if the user has the "Admin" or "Agency" role
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roleClaim != "Admin" && roleClaim != "Agency")
            {
                return Unauthorized("You are not authorized to view this user.");
            }

            return Ok(result.User);
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
    }
}
