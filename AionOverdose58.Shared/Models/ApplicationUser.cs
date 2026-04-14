using Microsoft.AspNetCore.Identity;

namespace AionOverdose58.Shared.Models;

/// <summary>
/// Extended IdentityUser for ASP.NET Core Identity.
/// Represents a web application user with additional Aion-specific properties.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// PIN code for account recovery (4-6 digits)
    /// </summary>
    public string? PinCode { get; set; }

    /// <summary>
    /// Date when the user registered
    /// </summary>
    public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Reference to the Aion game account UID
    /// </summary>
    public int? AionAccountUid { get; set; }

    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginDate { get; set; }

    /// <summary>
    /// Whether the user's email is verified
    /// </summary>
    public bool IsEmailVerified { get; set; } = false;

    /// <summary>
    /// Account status
    /// </summary>
    public bool IsActive { get; set; } = true;
}
