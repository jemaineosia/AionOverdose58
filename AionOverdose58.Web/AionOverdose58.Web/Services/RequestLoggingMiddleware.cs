using Serilog.Context;
using System.Diagnostics;

namespace AionOverdose58.Web.Services;

/// <summary>
/// Logs every HTTP request to Serilog with the authenticated username, client IP, and path.
/// Skips static assets (_framework, _content, css, js, images) to keep logs clean.
/// </summary>
public class RequestLoggingMiddleware
{
    private static readonly HashSet<string> _skipPrefixes = new(StringComparer.OrdinalIgnoreCase)
    {
        "/_framework", "/_content", "/css", "/js", "/images", "/fonts", "/favicon"
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? "/";

        // Skip static asset requests — no value logging those
        if (_skipPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        var username = context.User?.Identity?.IsAuthenticated == true
            ? context.User.Identity.Name ?? "Authenticated"
            : "Anonymous";

        var ip = context.Connection.RemoteIpAddress?.ToString()
              ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
              ?? "Unknown";

        // Strip IPv6 loopback
        if (ip == "::1") ip = "127.0.0.1";

        using var _ = LogContext.PushProperty("Username", username);
        using var __ = LogContext.PushProperty("IpAddress", ip);
        using var ___ = LogContext.PushProperty("Path", path);

        var sw = Stopwatch.StartNew();

        try
        {
            await _next(context);
            sw.Stop();

            _logger.LogInformation(
                "{Method} {Path} → {StatusCode} ({Elapsed}ms) | {Username} @ {IpAddress}",
                context.Request.Method,
                path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                username,
                ip);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex,
                "{Method} {Path} threw exception ({Elapsed}ms) | {Username} @ {IpAddress}",
                context.Request.Method,
                path,
                sw.ElapsedMilliseconds,
                username,
                ip);
            throw;
        }
    }
}
