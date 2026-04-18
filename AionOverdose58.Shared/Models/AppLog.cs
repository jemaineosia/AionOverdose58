namespace AionOverdose58.Shared.Models;

/// <summary>
/// Maps to the AppLogs table auto-created by Serilog.Sinks.MSSqlServer.
/// EF is used for read-only querying — Serilog owns the table.
/// </summary>
public class AppLog
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public string? Level { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? Exception { get; set; }
    public string? Username { get; set; }
    public string? IpAddress { get; set; }
    public string? Path { get; set; }
}
