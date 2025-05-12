using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Backend.WebSockets;

public class WebSocketManager
{
    private readonly ConcurrentDictionary<int, WebSocket> _userSockets = new();

    public async Task AddSocketAsync(int userId, WebSocket socket)
    {
        _userSockets[userId] = socket;
    }

    public async Task RemoveSocketAsync(int userId)
    {
        if (_userSockets.TryRemove(userId, out var socket))
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
        }
    }

    public async Task SendMessageToUserAsync(int userId, string message)
    {
        if (_userSockets.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _userSockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
