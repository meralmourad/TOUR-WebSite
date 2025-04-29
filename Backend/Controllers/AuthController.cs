using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs.UserDTOs;
using Backend.IServices;
using System;
// using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(IUserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] UserSignupDto user)
        {
            var result = await _userService.RegisterUserAsync(user);
            if (!result.Success) return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            var result = await _userService.LoginAsync(loginDto);

            if (!result.Success) return Unauthorized(result.Message);
            // Console.WriteLine("Login successful: " + result.User.Id + " Role: " + result.User.Role);
            var token = _jwtTokenService.GenerateToken(result.User.Id.ToString(), result.User.Role);
            return Ok(new { Token = token, User = result.User });
        }

        // [HttpGet("validate")]
        // public IActionResult ValidateToken([FromHeader] string authorization)
        // {
        //     if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
        //         return BadRequest("Invalid token format.");

        //     var token = authorization.Substring("Bearer ".Length);
        //     var principal = _jwtTokenService.ValidateToken(token);

        //     if (principal == null) return Unauthorized("Invalid or expired token.");
        //     return Ok("Token is valid.");
        // }
    }
}
