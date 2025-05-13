using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims; // Add this directive
using Backend.WebSockets;

namespace Backend.Controllers;

[ApiController]
[Route("ws")]
public class WebSocketController : ControllerBase
{
    private readonly WebSockets.WebSocketManager _webSocketManager;
    private readonly IConfiguration _configuration; // Inject configuration for JWT settings
    private readonly JwtTokenService _jwtTokenService; // Inject JwtTokenService

    public WebSocketController(WebSockets.WebSocketManager webSocketManager, IConfiguration configuration, JwtTokenService jwtTokenService)
    {
        _webSocketManager = webSocketManager;
        _configuration = configuration;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet("{userId}")]
    public async Task Get(int userId)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = 400;
            return;
        }

        // Extract the token from the query string
        var token = HttpContext.Request.Query["token"].ToString();
        if (string.IsNullOrEmpty(token) || !ValidateToken(token, out var validatedUserId))
        {
            HttpContext.Response.StatusCode = 401; // Unauthorized
            return;
        }

        // Ensure the userId matches the token's userId
        if (validatedUserId != userId)
        {
            Console.WriteLine($"UserId mismatch: Token userId {validatedUserId} does not match requested userId {userId}");
            HttpContext.Response.StatusCode = 403; // Forbidden
            return;
        }

        var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _webSocketManager.AddSocketAsync(userId, socket);

        // Keep the connection open
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.CloseStatus.HasValue)
            {
                await _webSocketManager.RemoveSocketAsync(userId);
            }
        }
    }

    private bool ValidateToken(string token, out int userId)
    {
        userId = 0;
        if (!_jwtTokenService.TryValidateToken(token, out var principal))
        {
            return false;
        }

        // Extract the userId claim
        var userIdClaim = principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier || x.Type == "nameid");
        if (userIdClaim == null)
        {
            Console.WriteLine("Token validation failed: 'nameid' claim is missing.");
            return false;
        }

        userId = int.Parse(userIdClaim.Value);
        Console.WriteLine($"Token validated successfully. UserId: {userId}");
        return true;
    }
}
