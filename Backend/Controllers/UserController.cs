using Backend.Data;
using Backend.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        public UserController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;         
        }
        // GET: api/user
        [HttpGet]
    public async Task<IActionResult> GetUsers(){
        var users =await _unitOfWork.User.GetAllAsync();
        if (users == null || !users.Any())
        {
                return NotFound("No users found.");
        }
        var userDtos = users.Select(user => new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address?.ToString(),
            Role = user.Role
        }).ToList();
        return Ok(userDtos);
    }
        // POST: api/user
        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] UserSignupDto user){
            if (user.Password != user.ConfirmPassword)
                return BadRequest("Password and Confirm Password do not match.");

        var userEntity = UserSignupDto.ToDb(user); 
        var isUserExists = await _unitOfWork.User.GetUserByEmailAsync(userEntity.Email);
        if (isUserExists != null)
            return BadRequest("User with this email already exists.");
        
        await _unitOfWork.User.AddAsync(userEntity);
        await _unitOfWork.CompleteAsync(); 
        return CreatedAtAction(nameof(GetUsers), new { id = userEntity.Id }, user);
    }
    //login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto)
    {
        var user = await _unitOfWork.User.GetUserByEmailAsync(userLoginDto.Email);
        if (user == null)
            return NotFound("User not found.");
        if (user.Password != userLoginDto.Password)
            return Unauthorized("Invalid password.");
        var userDto = new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address?.ToString(),
            Role = user.Role
        };
        return Ok(userDto);
    }
        // PUT: api/user/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto user)
    {
        if (id != user.Id) return BadRequest("User ID mismatch.");
        var existingUser = await _unitOfWork.User.GetByIdAsync(id);
        if (existingUser == null)
            return NotFound("User not found.");
        // Update logic here    
        return NoContent();
    }

    // DELETE: api/user/5
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
        {
            var user = _unitOfWork.User.GetByIdAsync(id).Result;
            if (user == null)
                return NotFound("User not found.");
            _unitOfWork.User.Delete(user);
            _unitOfWork.CompleteAsync().Wait();     
            return NoContent();
        }
    }
}

//------------------------------------------------
// TODO :
// hash password