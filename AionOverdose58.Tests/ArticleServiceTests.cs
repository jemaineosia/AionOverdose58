using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using AionOverdose58.Web.Services;

namespace AionOverdose58.Tests;

public class ArticleServiceTests
{
    // ── Factory helper: each test gets its own isolated DB ─────────────────
    private static ArticleService Build(string db)
        => new(DbHelper.CreateFactory(db));

    private static Article Sample(string title = "Test Article",
                                  ArticleCategory cat = ArticleCategory.Announcement)
        => new() { Title = title, Category = cat, PreviewContent = "preview", Content = "<p>body</p>" };

    // ════════════════════════════════════════════════════════════════════════
    //  CreateAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task CreateAsync_PersistsArticleAndReturnsWithId()
    {
        var svc     = Build(nameof(CreateAsync_PersistsArticleAndReturnsWithId));
        var created = await svc.CreateAsync(Sample("Hello World"));

        Assert.True(created.Id > 0);
        Assert.Equal("Hello World", created.Title);
    }

    [Fact]
    public async Task CreateAsync_SetsCreatedOnAndUpdatedOn()
    {
        var before  = DateTime.UtcNow.AddSeconds(-1);
        var svc     = Build(nameof(CreateAsync_SetsCreatedOnAndUpdatedOn));
        var created = await svc.CreateAsync(Sample());

        Assert.True(created.CreatedOn >= before);
        Assert.True(created.UpdatedOn >= before);
    }

    [Fact]
    public async Task CreateAsync_DefaultIsDeletedFalse()
    {
        var svc     = Build(nameof(CreateAsync_DefaultIsDeletedFalse));
        var created = await svc.CreateAsync(Sample());

        Assert.False(created.IsDeleted);
    }

    // ════════════════════════════════════════════════════════════════════════
    //  GetAllAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyNonDeletedByDefault()
    {
        var svc = Build(nameof(GetAllAsync_ReturnsOnlyNonDeletedByDefault));
        var a1  = await svc.CreateAsync(Sample("Active"));
        var a2  = await svc.CreateAsync(Sample("ToDelete"));
        await svc.SoftDeleteAsync(a2.Id);

        var results = await svc.GetAllAsync();

        Assert.Single(results);
        Assert.Equal("Active", results[0].Title);
    }

    [Fact]
    public async Task GetAllAsync_IncludesDeletedWhenFlagSet()
    {
        var svc = Build(nameof(GetAllAsync_IncludesDeletedWhenFlagSet));
        var a   = await svc.CreateAsync(Sample());
        await svc.SoftDeleteAsync(a.Id);

        var results = await svc.GetAllAsync(includeDeleted: true);

        Assert.Single(results);
        Assert.True(results[0].IsDeleted);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsNewestFirst()
    {
        var svc = Build(nameof(GetAllAsync_ReturnsNewestFirst));
        var a1  = await svc.CreateAsync(Sample("Old"));
        // Force a later CreatedOn
        await Task.Delay(10);
        var a2  = await svc.CreateAsync(Sample("New"));

        var results = await svc.GetAllAsync();

        Assert.Equal("New", results[0].Title);
    }

    // ════════════════════════════════════════════════════════════════════════
    //  GetByCategoryAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetByCategoryAsync_FiltersCorrectly()
    {
        var svc = Build(nameof(GetByCategoryAsync_FiltersCorrectly));
        await svc.CreateAsync(Sample(cat: ArticleCategory.Announcement));
        await svc.CreateAsync(Sample(cat: ArticleCategory.PatchNotes));
        await svc.CreateAsync(Sample(cat: ArticleCategory.PatchNotes));

        var patches = await svc.GetByCategoryAsync(ArticleCategory.PatchNotes);

        Assert.Equal(2, patches.Count);
        Assert.All(patches, a => Assert.Equal(ArticleCategory.PatchNotes, a.Category));
    }

    [Fact]
    public async Task GetByCategoryAsync_ExcludesDeletedByDefault()
    {
        var svc = Build(nameof(GetByCategoryAsync_ExcludesDeletedByDefault));
        var a   = await svc.CreateAsync(Sample(cat: ArticleCategory.Sales));
        await svc.SoftDeleteAsync(a.Id);

        var results = await svc.GetByCategoryAsync(ArticleCategory.Sales);

        Assert.Empty(results);
    }

    // ════════════════════════════════════════════════════════════════════════
    //  GetByIdAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectArticle()
    {
        var svc     = Build(nameof(GetByIdAsync_ReturnsCorrectArticle));
        var created = await svc.CreateAsync(Sample("Find Me"));

        var found = await svc.GetByIdAsync(created.Id);

        Assert.NotNull(found);
        Assert.Equal("Find Me", found!.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullForMissingId()
    {
        var svc    = Build(nameof(GetByIdAsync_ReturnsNullForMissingId));
        var result = await svc.GetByIdAsync(9999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSoftDeletedArticle()
    {
        var svc     = Build(nameof(GetByIdAsync_ReturnsSoftDeletedArticle));
        var created = await svc.CreateAsync(Sample());
        await svc.SoftDeleteAsync(created.Id);

        var found = await svc.GetByIdAsync(created.Id);

        Assert.NotNull(found);
        Assert.True(found!.IsDeleted);
    }

    // ════════════════════════════════════════════════════════════════════════
    //  UpdateAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var svc     = Build(nameof(UpdateAsync_PersistsChanges));
        var created = await svc.CreateAsync(Sample("Original"));

        created.Title = "Updated";
        await svc.UpdateAsync(created);

        var fetched = await svc.GetByIdAsync(created.Id);
        Assert.Equal("Updated", fetched!.Title);
    }

    [Fact]
    public async Task UpdateAsync_BumpsUpdatedOn()
    {
        var svc         = Build(nameof(UpdateAsync_BumpsUpdatedOn));
        var created     = await svc.CreateAsync(Sample());
        var beforeUpdate = created.UpdatedOn;

        await Task.Delay(10);
        await svc.UpdateAsync(created);

        var fetched = await svc.GetByIdAsync(created.Id);
        Assert.True(fetched!.UpdatedOn >= beforeUpdate);
    }

    // ════════════════════════════════════════════════════════════════════════
    //  SoftDeleteAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task SoftDeleteAsync_SetsIsDeletedTrue()
    {
        var svc     = Build(nameof(SoftDeleteAsync_SetsIsDeletedTrue));
        var created = await svc.CreateAsync(Sample());

        var result = await svc.SoftDeleteAsync(created.Id);

        Assert.True(result);
        var fetched = await svc.GetByIdAsync(created.Id);
        Assert.True(fetched!.IsDeleted);
    }

    [Fact]
    public async Task SoftDeleteAsync_ReturnsFalseForMissingId()
    {
        var svc    = Build(nameof(SoftDeleteAsync_ReturnsFalseForMissingId));
        var result = await svc.SoftDeleteAsync(9999);

        Assert.False(result);
    }

    // ════════════════════════════════════════════════════════════════════════
    //  RestoreAsync
    // ════════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task RestoreAsync_ClearsIsDeleted()
    {
        var svc     = Build(nameof(RestoreAsync_ClearsIsDeleted));
        var created = await svc.CreateAsync(Sample());
        await svc.SoftDeleteAsync(created.Id);

        var result  = await svc.RestoreAsync(created.Id);

        Assert.True(result);
        var fetched = await svc.GetByIdAsync(created.Id);
        Assert.False(fetched!.IsDeleted);
    }

    [Fact]
    public async Task RestoreAsync_ReturnsFalseForMissingId()
    {
        var svc    = Build(nameof(RestoreAsync_ReturnsFalseForMissingId));
        var result = await svc.RestoreAsync(9999);

        Assert.False(result);
    }

    [Fact]
    public async Task RestoreAsync_ArticleAppearsInNormalQueryAgain()
    {
        var svc     = Build(nameof(RestoreAsync_ArticleAppearsInNormalQueryAgain));
        var created = await svc.CreateAsync(Sample("Revival"));
        await svc.SoftDeleteAsync(created.Id);

        // verify it's gone from normal query
        Assert.Empty(await svc.GetAllAsync());

        await svc.RestoreAsync(created.Id);

        var results = await svc.GetAllAsync();
        Assert.Single(results);
        Assert.Equal("Revival", results[0].Title);
    }
}
