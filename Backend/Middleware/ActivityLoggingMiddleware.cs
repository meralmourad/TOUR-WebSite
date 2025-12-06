using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Backend.Middleware;

public class ActivityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActivityLoggingMiddleware> _logger;

    public ActivityLoggingMiddleware(RequestDelegate next, ILogger<ActivityLoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];
        
        // Extract request information
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;
        var queryString = context.Request.QueryString.ToString();
        var clientIp = GetClientIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        
        // Log the incoming request
        _logger.LogInformation(
            "[{RequestId}] Incoming {Method} request to {Path}{Query} from {ClientIp}",
            requestId, requestMethod, requestPath, queryString, clientIp);

        // Store original response body stream
        var originalBodyStream = context.Response.Body;
        
        try
        {
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();

            // Get user info after authentication middleware has run
            var userId = GetUserId(context);
            var userRole = GetUserRole(context);
            var statusCode = context.Response.StatusCode;
            var contentLength = context.Response.ContentLength ?? responseBody.Length;

            // Log based on response status
            if (statusCode >= 500)
            {
                _logger.LogError(
                    "[{RequestId}] {Method} {Path} responded {StatusCode} in {ElapsedMs}ms | User: {UserId} ({Role}) | IP: {ClientIp} | Size: {ContentLength}B",
                    requestId, requestMethod, requestPath, statusCode, stopwatch.ElapsedMilliseconds, 
                    userId, userRole, clientIp, contentLength);
            }
            else if (statusCode >= 400)
            {
                _logger.LogWarning(
                    "[{RequestId}] {Method} {Path} responded {StatusCode} in {ElapsedMs}ms | User: {UserId} ({Role}) | IP: {ClientIp}",
                    requestId, requestMethod, requestPath, statusCode, stopwatch.ElapsedMilliseconds, 
                    userId, userRole, clientIp);
            }
            else
            {
                _logger.LogInformation(
                    "[{RequestId}] {Method} {Path} responded {StatusCode} in {ElapsedMs}ms | User: {UserId} ({Role}) | IP: {ClientIp}",
                    requestId, requestMethod, requestPath, statusCode, stopwatch.ElapsedMilliseconds, 
                    userId, userRole, clientIp);
            }

            // Log specific activities
            LogSpecificActivity(requestMethod, requestPath, statusCode, userId, userRole, requestId);

            // Copy the response back to the original stream
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var userId = GetUserId(context);
            
            _logger.LogError(ex,
                "[{RequestId}] {Method} {Path} threw exception after {ElapsedMs}ms | User: {UserId} | IP: {ClientIp} | Error: {ErrorMessage}",
                requestId, requestMethod, requestPath, stopwatch.ElapsedMilliseconds, 
                userId, clientIp, ex.Message);

            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private void LogSpecificActivity(string method, PathString path, int statusCode, string userId, string userRole, string requestId)
    {
        var pathLower = path.ToString().ToLower();

        // Authentication activities
        if (pathLower.Contains("/auth/login") && method == "POST")
        {
            if (statusCode == 200)
                _logger.LogInformation("[{RequestId}] AUTH: User login successful", requestId);
            else
                _logger.LogWarning("[{RequestId}] AUTH: Failed login attempt", requestId);
        }
        else if (pathLower.Contains("/auth/signup") && method == "POST")
        {
            if (statusCode == 200)
                _logger.LogInformation("[{RequestId}] AUTH: New user registration successful", requestId);
            else
                _logger.LogWarning("[{RequestId}] AUTH: Failed registration attempt", requestId);
        }
        // Booking activities
        else if (pathLower.Contains("/booking") && method == "POST" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] BOOKING: User {UserId} ({Role}) created a new booking", requestId, userId, userRole);
        }
        else if (pathLower.Contains("/booking") && method == "DELETE" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] BOOKING: User {UserId} ({Role}) cancelled a booking", requestId, userId, userRole);
        }
        // Trip activities
        else if (pathLower.Contains("/trip") && method == "POST" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] TRIP: User {UserId} ({Role}) created a new trip", requestId, userId, userRole);
        }
        else if (pathLower.Contains("/trip") && method == "PUT" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] TRIP: User {UserId} ({Role}) updated a trip", requestId, userId, userRole);
        }
        else if (pathLower.Contains("/trip") && method == "DELETE" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] TRIP: User {UserId} ({Role}) deleted a trip", requestId, userId, userRole);
        }
        // Place activities
        else if (pathLower.Contains("/place") && method == "POST" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] PLACE: User {UserId} ({Role}) created a new place", requestId, userId, userRole);
        }
        // User management activities
        else if (pathLower.Contains("/user") && method == "PUT" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] USER: User {UserId} ({Role}) updated user profile", requestId, userId, userRole);
        }
        else if (pathLower.Contains("/user") && method == "DELETE" && statusCode < 400)
        {
            _logger.LogWarning("[{RequestId}] USER: User {UserId} ({Role}) deleted a user account", requestId, userId, userRole);
        }
        // Message activities
        else if (pathLower.Contains("/message") && method == "POST" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] MESSAGE: User {UserId} sent a message", requestId, userId);
        }
        // Report activities
        else if (pathLower.Contains("/report") && method == "POST" && statusCode < 400)
        {
            _logger.LogInformation("[{RequestId}] REPORT: User {UserId} ({Role}) submitted a report", requestId, userId, userRole);
        }
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded headers first (for reverse proxy scenarios)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var remoteIp = context.Connection.RemoteIpAddress;
        if (remoteIp == null) return "unknown";
        
        try
        {
            return remoteIp.MapToIPv4().ToString();
        }
        catch
        {
            return remoteIp.ToString();
        }
    }

    private static string GetUserId(HttpContext context)
    {
        return context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
               ?? context.User?.FindFirst("sub")?.Value 
               ?? "anonymous";
    }

    private static string GetUserRole(HttpContext context)
    {
        return context.User?.FindFirst(ClaimTypes.Role)?.Value 
               ?? context.User?.FindFirst("role")?.Value 
               ?? "none";
    }
}
