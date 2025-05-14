using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims; // Add this directive
using Backend.WebSockets;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.Logging;

namespace Backend.Controllers;

[ApiController]
[Route("ws")]
public class WebSocketController : ControllerBase
{
    private readonly MyWebSocketManager _webSocketManager;
    readonly notificationSocket _notificationSocket;
    private readonly JwtTokenService _jwtTokenService;
    private readonly ILogger<WebSocketController> _logger;

    public WebSocketController(MyWebSocketManager webSocketManager, JwtTokenService jwtTokenService, ILogger<WebSocketController> logger, notificationSocket notificationSocket)
    {
        _notificationSocket = notificationSocket;
        _webSocketManager = webSocketManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    [HttpGet("chat/{userId}")]
    public async Task Get(int userId)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = 400;
            return;
        }

        // Extract the token from the query string
        var token = HttpContext.Request.Query["token"].ToString();
        _logger.LogInformation($"Received token: {token}");

        var principal = _jwtTokenService.ValidateToken(token);
        if (principal == null)
        {
            HttpContext.Response.StatusCode = 401; // Unauthorized
            return;
        }

        // Ensure the userId matches the token's userId
        var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || int.Parse(userIdClaim) != userId)
        {
            _logger.LogWarning($"UserId mismatch: Token userId {userIdClaim} does not match requested userId {userId}");
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

    // endpont to the notification websocket
    [HttpGet("notification/{userId}")]
    public async Task GetNotification(int userId)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = 400;
            return;
        }

        // Extract the token from the query string
        var token = HttpContext.Request.Query["token"].ToString();
        _logger.LogInformation($"Received token: {token}");

        var principal = _jwtTokenService.ValidateToken(token);
        if (principal == null)
        {
            HttpContext.Response.StatusCode = 401; // Unauthorized
            return;
        }

        // Ensure the userId matches the token's userId
        var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || int.Parse(userIdClaim) != userId)
        {
            _logger.LogWarning($"UserId mismatch: Token userId {userIdClaim} does not match requested userId {userId}");
            HttpContext.Response.StatusCode = 403; // Forbidden
            return;
        }

        var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _notificationSocket.AddSocketAsync(userId, socket);

        // Keep the connection open
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.CloseStatus.HasValue)
            {
                await _notificationSocket.RemoveSocketAsync(userId);
            }
        }
    }
}
