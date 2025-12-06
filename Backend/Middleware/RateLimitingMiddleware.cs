using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Backend.Middleware;

internal class RateLimitState
{
    public int Count { get; set; }
    public DateTimeOffset ResetAt { get; set; }
}

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly int _limit;
    private readonly int _windowSeconds;

    public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, IConfiguration config)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        _limit = int.TryParse(config["RateLimiting:Requests"], out var l) ? l : 60;
        _windowSeconds = int.TryParse(config["RateLimiting:WindowSeconds"], out var w) ? w : 60;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Determine key: prefer authenticated user id, otherwise client IP
        string key;
        var user = context.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            // Use NameIdentifier or fallback to Name
            var id = user.FindFirst("sub")?.Value ?? user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? user.Identity?.Name ?? "anon";
            key = $"rl_user_{id}";
        }
        else
        {
            // Normalize IP (prefer IPv4 mapped) to avoid ::1 vs 127.0.0.1 creating different keys
            var remote = context.Connection.RemoteIpAddress;
            string ip;
            if (remote == null)
                ip = "unknown";
            else
            {
                try
                {
                    var ipv4 = remote.MapToIPv4();
                    ip = ipv4.ToString();
                }
                catch
                {
                    ip = remote.ToString();
                }
            }

            key = $"rl_ip_{ip}";
        }

        var now = DateTimeOffset.UtcNow;

        var state = _cache.GetOrCreate(key, entry =>
        {
            var s = new RateLimitState
            {
                Count = 0,
                ResetAt = now.AddSeconds(_windowSeconds)
            };
            entry.AbsoluteExpiration = s.ResetAt;
            return s;
        });

        state.Count++;
        // Update cache so expiration persists until ResetAt
        _cache.Set(key, state, new MemoryCacheEntryOptions { AbsoluteExpiration = state.ResetAt });

        var remaining = Math.Max(0, _limit - state.Count);

        context.Response.Headers["X-RateLimit-Limit"] = _limit.ToString();
        context.Response.Headers["X-RateLimit-Remaining"] = remaining.ToString();
        context.Response.Headers["X-RateLimit-Reset"] = ((int)(state.ResetAt - now).TotalSeconds).ToString();

        if (state.Count > _limit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = Math.Max(1, (int)(state.ResetAt - now).TotalSeconds).ToString();
            await context.Response.WriteAsync("Too many requests. Please try again later.");
            return;
        }

        await _next(context);
    }
}
