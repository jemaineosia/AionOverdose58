using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Web.Services;

// ──────────────────────────────────────────
//  Interface
// ──────────────────────────────────────────
public interface IArticleService
{
    Task<List<Article>> GetAllAsync(bool includeDeleted = false);
    Task<List<Article>> GetByCategoryAsync(ArticleCategory category, bool includeDeleted = false);
    Task<Article?>      GetByIdAsync(int id);
    Task<Article>       CreateAsync(Article article);
    Task<Article>       UpdateAsync(Article article);
    Task<bool>          SoftDeleteAsync(int id);
    Task<bool>          RestoreAsync(int id);
}

// ──────────────────────────────────────────
//  Implementation
// ──────────────────────────────────────────
public class ArticleService : IArticleService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public ArticleService(IDbContextFactory<AppDbContext> dbFactory)
        => _dbFactory = dbFactory;

    /// <summary>Returns all articles, optionally including soft-deleted ones.</summary>
    public async Task<List<Article>> GetAllAsync(bool includeDeleted = false)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        IQueryable<Article> query = db.Articles;
        if (!includeDeleted) query = query.Where(a => !a.IsDeleted);
        return await query.OrderByDescending(a => a.CreatedOn).ToListAsync();
    }

    /// <summary>Returns articles filtered by category.</summary>
    public async Task<List<Article>> GetByCategoryAsync(ArticleCategory category, bool includeDeleted = false)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        IQueryable<Article> query = db.Articles.Where(a => a.Category == category);
        if (!includeDeleted) query = query.Where(a => !a.IsDeleted);
        return await query.OrderByDescending(a => a.CreatedOn).ToListAsync();
    }

    /// <summary>Returns a single article by ID (including deleted).</summary>
    public async Task<Article?> GetByIdAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Articles.FirstOrDefaultAsync(a => a.Id == id);
    }

    /// <summary>Persists a new article and returns it with the generated ID.</summary>
    public async Task<Article> CreateAsync(Article article)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        article.CreatedOn = DateTime.UtcNow;
        article.UpdatedOn = DateTime.UtcNow;
        db.Articles.Add(article);
        await db.SaveChangesAsync();
        return article;
    }

    /// <summary>Updates an existing article.</summary>
    public async Task<Article> UpdateAsync(Article article)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        article.UpdatedOn = DateTime.UtcNow;
        db.Articles.Update(article);
        await db.SaveChangesAsync();
        return article;
    }

    /// <summary>Soft-deletes an article (sets IsDeleted = true).</summary>
    public async Task<bool> SoftDeleteAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var article = await db.Articles.FindAsync(id);
        if (article is null) return false;
        article.IsDeleted = true;
        article.UpdatedOn = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    /// <summary>Restores a soft-deleted article.</summary>
    public async Task<bool> RestoreAsync(int id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var article = await db.Articles.FindAsync(id);
        if (article is null) return false;
        article.IsDeleted = false;
        article.UpdatedOn = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }
}
