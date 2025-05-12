using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.WebSockets;

namespace Backend.Controllers;

[ApiController]
[Route("ws")]
public class WebSocketController : ControllerBase
{
    private readonly WebSockets.WebSocketManager _webSocketManager;
    private readonly IConfiguration _configuration; // Inject configuration for JWT settings

    public WebSocketController(WebSockets.WebSocketManager webSocketManager, IConfiguration configuration)
    {
        _webSocketManager = webSocketManager;
        _configuration = configuration;
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
        try
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value); // Assuming "nameid" contains the userId
            return true;
        }
        catch
        {
            return false;
        }
    }
}
