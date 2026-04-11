namespace AionOverdose58.Shared.Models;

public class PlayerRanking
{
    public int Id { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public int Level { get; set; }
    public long Score { get; set; }
    public int Rank { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
