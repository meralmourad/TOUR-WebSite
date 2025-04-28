using Backend.Data;
using Backend.DTOs.UserDTOs;
using Backend.IServices;
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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any()) return NotFound("No users found.");
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            return success ? NoContent() : NotFound("User not found.");
        }
        //update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userUpdateDto,int id)
        {
            var result = await _userService.UpdateUserAsync(userUpdateDto, id);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Success);
        }
    }
}
