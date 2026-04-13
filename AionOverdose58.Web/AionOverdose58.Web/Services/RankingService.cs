using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Web.Services;

public interface IRankingService
{
    Task<List<object>> GetRankingsAsync();
}

public class RankingService : IRankingService
{
    // Rankings table removed for now — returns empty list until re-added
    public Task<List<object>> GetRankingsAsync() => Task.FromResult(new List<object>());
}
