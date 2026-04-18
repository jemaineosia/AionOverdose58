using Serilog.Context;

namespace AionOverdose58.Web.Services;

/// <summary>
/// Structured audit logger for identity form submissions.
/// Writes through Serilog so events land in AppLogs with Username / IpAddress / Path columns.
/// </summary>
public interface IAuditLogger
{
    /// <summary>
    /// Log a form submission event.
    /// </summary>
    /// <param name="action">Short camelCase action name, e.g. "Login", "Register", "ChangePassword"</param>
    /// <param name="success">Whether the form action succeeded</param>
    /// <param name="username">Identity username or email involved (never a raw password)</param>
    /// <param name="ipAddress">Client IP address</param>
    /// <param name="detail">Optional human-readable outcome detail (never a raw password)</param>
    void LogFormEvent(string action, bool success,
        string? username = null, string? ipAddress = null, string? detail = null);
}

public class AuditLogger : IAuditLogger
{
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(ILogger<AuditLogger> logger) => _logger = logger;

    public void LogFormEvent(string action, bool success,
        string? username = null, string? ipAddress = null, string? detail = null)
    {
        var user = string.IsNullOrWhiteSpace(username) ? "Anonymous" : username;
        var ip   = string.IsNullOrWhiteSpace(ipAddress) ? "Unknown"   : ipAddress;
        var path = $"/form/{action}";

        using var _u = LogContext.PushProperty("Username",  user);
        using var _i = LogContext.PushProperty("IpAddress", ip);
        using var _p = LogContext.PushProperty("Path",      path);

        var status = success ? "SUCCESS" : "FAILED";
        var detailStr = string.IsNullOrWhiteSpace(detail) ? string.Empty : $" — {detail}";

        if (success)
            _logger.LogInformation("[FORM] {Action} {Status}{Detail} | {User} @ {Ip}",
                action, status, detailStr, user, ip);
        else
            _logger.LogWarning("[FORM] {Action} {Status}{Detail} | {User} @ {Ip}",
                action, status, detailStr, user, ip);
    }
}
