using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Web.Services;

public interface IRankingService
{
    Task<List<PlayerRanking>> GetRankingsAsync();
}

public class RankingService : IRankingService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    private static readonly List<PlayerRanking> _fallback = new()
    {
        new PlayerRanking { Id = 1, PlayerName = "DarkWingDaeva", Class = "Gladiator", Level = 65, Score = 1_250_000, Rank = 1, UpdatedAt = DateTime.UtcNow },
        new PlayerRanking { Id = 2, PlayerName = "SkyBlazer", Class = "Ranger", Level = 65, Score = 1_180_500, Rank = 2, UpdatedAt = DateTime.UtcNow },
        new PlayerRanking { Id = 3, PlayerName = "AbyssQueen", Class = "Sorcerer", Level = 65, Score = 1_050_200, Rank = 3, UpdatedAt = DateTime.UtcNow },
        new PlayerRanking { Id = 4, PlayerName = "IronTemplar", Class = "Templar", Level = 64, Score = 987_300, Rank = 4, UpdatedAt = DateTime.UtcNow },
        new PlayerRanking { Id = 5, PlayerName = "StormChaser", Class = "Assassin", Level = 63, Score = 912_750, Rank = 5, UpdatedAt = DateTime.UtcNow },
    };

    public RankingService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<PlayerRanking>> GetRankingsAsync()
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.PlayerRankings
                .OrderBy(r => r.Rank)
                .ToListAsync();
        }
        catch
        {
            return _fallback;
        }
    }
}
