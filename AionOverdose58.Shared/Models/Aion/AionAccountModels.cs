namespace AionOverdose58.Shared.Models.Aion;

/// <summary>
/// Represents an account in the Aion accounts database (account_data table).
/// </summary>
public class AionAccountData
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte[] Password { get; set; } = Array.Empty<byte>();
    public int AccessLevel { get; set; } = 0;
    public byte MembershipLevel { get; set; } = 0;
    public long Toll { get; set; } = 0;
    public DateTime? LastServer { get; set; }
    public string? LastIp { get; set; }
    public string? IpForce { get; set; }
    public byte[] Activated { get; set; } = new byte[] { 1 };
}

/// <summary>
/// Represents account time information (account_time table).
/// </summary>
public class AionAccountTime
{
    public int AccountId { get; set; }
    public DateTime? LastActive { get; set; }
    public int? ExpansionPass { get; set; }
    public long? PenaltyEnd { get; set; }
}

/// <summary>
/// Represents SSN/Email information (ssn table) - Used by stored procedure.
/// </summary>
public class AionSsn
{
    public int Id { get; set; }
    public string Ssn { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Newsletter { get; set; } = 0;
    public int Job { get; set; } = 0;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public DateTime? RegDate { get; set; }
    public string? Zip { get; set; }
    public string? AddrMain { get; set; }
    public string? AddrEtc { get; set; }
    public int AccountNum { get; set; }
    public int StatusFlag { get; set; } = 0;
}

/// <summary>
/// Represents user account (user_account table) - Used by stored procedure.
/// </summary>
public class AionUserAccount
{
    public int Uid { get; set; }
    public string Account { get; set; } = string.Empty;
    public int PayStat { get; set; } = 1;
}

/// <summary>
/// Represents user authentication (user_auth table) - Used by stored procedure.
/// </summary>
public class AionUserAuth
{
    public string Account { get; set; } = string.Empty;
    public byte[] Password { get; set; } = Array.Empty<byte>();
    public string? Quiz1 { get; set; }
    public string? Quiz2 { get; set; }
    public byte[]? Answer1 { get; set; }
    public byte[]? Answer2 { get; set; }
    public int NewPwdFlag { get; set; } = 0;
    public string? Passwd { get; set; }
    public string? WebPassword { get; set; }
    public int WebLevel { get; set; } = 0;
}

/// <summary>
/// Represents user info (user_info table) - Used by stored procedure.
/// </summary>
public class AionUserInfo
{
    public string Account { get; set; } = string.Empty;
    public DateTime? CreateDate { get; set; }
    public string? Ssn { get; set; }
    public int StatusFlag { get; set; } = 0;
    public int Kind { get; set; } = 99;
}
