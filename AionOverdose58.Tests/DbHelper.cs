using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using AionOverdose58.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Tests;

/// <summary>
/// Helper that spins up a fresh in-memory AppDbContext per test.
/// </summary>
internal static class DbHelper
{
    public static AppDbContext CreateInMemory(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new AppDbContext(options);
    }

    /// <summary>Wraps a single AppDbContext in an IDbContextFactory.</summary>
    public static IDbContextFactory<AppDbContext> CreateFactory(string dbName)
        => new TestDbContextFactory(dbName);

    private sealed class TestDbContextFactory(string dbName) : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext() => DbHelper.CreateInMemory(dbName);
        public Task<AppDbContext> CreateDbContextAsync(CancellationToken _ = default)
            => Task.FromResult(CreateDbContext());
    }
}
