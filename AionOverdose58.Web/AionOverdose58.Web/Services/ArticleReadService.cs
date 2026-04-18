using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Web.Services;

public interface IArticleReadService
{
    Task<List<Article>> GetLatestAsync(int count = 3);
    Task<List<Article>> GetAllAsync(int page = 1, int pageSize = 9, ArticleCategory? category = null);
    Task<int> GetTotalCountAsync(ArticleCategory? category = null);
    Task<Article?> GetByIdAsync(int id);
}

public class ArticleReadService : IArticleReadService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public ArticleReadService(IDbContextFactory<AppDbContext> dbContextFactory)
        => _dbContextFactory = dbContextFactory;

    public async Task<List<Article>> GetLatestAsync(int count = 3)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.Articles
                .Where(a => !a.IsDeleted && a.PostedDate != null)
                .OrderByDescending(a => a.PostedDate)
                .Take(count)
                .ToListAsync();
        }
        catch { return []; }
    }

    public async Task<List<Article>> GetAllAsync(int page = 1, int pageSize = 9, ArticleCategory? category = null)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var query = db.Articles.Where(a => !a.IsDeleted && a.PostedDate != null);

            if (category is not null)
                query = query.Where(a => a.Category == category);

            return await query
                .OrderByDescending(a => a.PostedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        catch { return []; }
    }

    public async Task<int> GetTotalCountAsync(ArticleCategory? category = null)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var query = db.Articles.Where(a => !a.IsDeleted && a.PostedDate != null);

            if (category is not null)
                query = query.Where(a => a.Category == category);

            return await query.CountAsync();
        }
        catch { return 0; }
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return await db.Articles
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }
        catch { return null; }
    }
}
