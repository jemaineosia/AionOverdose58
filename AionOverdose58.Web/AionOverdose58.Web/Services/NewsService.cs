using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Web.Services;

public interface INewsService
{
    Task<List<NewsArticle>> GetLatestAsync(int count = 3);
    Task<List<NewsArticle>> GetAllAsync(int page = 1, int pageSize = 9);
    Task<int> GetTotalCountAsync();
    Task<NewsArticle?> GetBySlugAsync(string slug);
}

public class NewsService : INewsService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    private static readonly List<NewsArticle> _fallback = new()
    {
        new NewsArticle
        {
            Id = 1,
            Title = "Aion 2 Early Access Begins — The Eternal Sky Awakens",
            Slug = "aion2-early-access-begins",
            Summary = "The gates of the Abyss have opened. Early access to Aion 2 is now live, bringing sweeping new mechanics, redesigned classes, and breathtaking visuals.",
            Content = "<h2>Aion 2 Early Access Begins</h2><p>The wait is finally over. After years of anticipation, <strong>Aion 2</strong> has officially entered Early Access.</p>",
            ImageUrl = "https://picsum.photos/800/450?random=10",
            PublishedAt = new DateTime(2025, 3, 15, 10, 0, 0, DateTimeKind.Utc),
            AuthorName = "AionOverdose Staff",
            IsPublished = true
        },
        new NewsArticle
        {
            Id = 2,
            Title = "New Class Reveal: The Shadow Reaper Joins the Battle",
            Slug = "shadow-reaper-class-reveal",
            Summary = "NCSoft reveals the devastating Shadow Reaper — a dark melee-ranged hybrid class wielding twin soul-swords.",
            Content = "<h2>Shadow Reaper — Class Overview</h2><p>The <strong>Shadow Reaper</strong> is a fearsome Asmodian-aligned class that blends melee ferocity with dark magic.</p>",
            ImageUrl = "https://picsum.photos/800/450?random=11",
            PublishedAt = new DateTime(2025, 2, 28, 14, 0, 0, DateTimeKind.Utc),
            AuthorName = "AionOverdose Staff",
            IsPublished = true
        },
        new NewsArticle
        {
            Id = 3,
            Title = "Server Maintenance & Balance Patch 1.3 Notes",
            Slug = "server-maintenance-balance-patch-1-3",
            Summary = "Scheduled maintenance on Saturday — patch 1.3 brings critical balance changes to PvP combat and Fortress siege timers.",
            Content = "<h2>Patch 1.3 — Balance & Maintenance</h2><p>Maintenance window: <strong>Saturday 02:00 – 06:00 UTC</strong></p>",
            ImageUrl = "https://picsum.photos/800/450?random=12",
            PublishedAt = new DateTime(2025, 2, 10, 9, 0, 0, DateTimeKind.Utc),
            AuthorName = "AionOverdose Staff",
            IsPublished = true
        }
    };

    public NewsService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<NewsArticle>> GetLatestAsync(int count = 3)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.NewsArticles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishedAt)
                .Take(count)
                .ToListAsync();
        }
        catch
        {
            return _fallback.Take(count).ToList();
        }
    }

    public async Task<List<NewsArticle>> GetAllAsync(int page = 1, int pageSize = 9)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.NewsArticles
                .Where(a => a.IsPublished)
                .OrderByDescending(a => a.PublishedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch
        {
            return _fallback.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.NewsArticles.CountAsync(a => a.IsPublished);
        }
        catch
        {
            return _fallback.Count;
        }
    }

    public async Task<NewsArticle?> GetBySlugAsync(string slug)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.NewsArticles
                .FirstOrDefaultAsync(a => a.Slug == slug && a.IsPublished);
        }
        catch
        {
            return _fallback.FirstOrDefault(a => a.Slug == slug);
        }
    }
}
