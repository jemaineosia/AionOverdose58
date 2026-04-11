using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();
    public DbSet<WikiEntry> WikiEntries => Set<WikiEntry>();
    public DbSet<PlayerRanking> PlayerRankings => Set<PlayerRanking>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NewsArticle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(300);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.AuthorName).HasMaxLength(100);
        });

        modelBuilder.Entity<WikiEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(300);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.Category).HasMaxLength(100);
        });

        modelBuilder.Entity<PlayerRanking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PlayerName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Class).HasMaxLength(50);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NewsArticle>().HasData(
            new NewsArticle
            {
                Id = 1,
                Title = "Aion 2 Early Access Begins — The Eternal Sky Awakens",
                Slug = "aion2-early-access-begins",
                Summary = "The gates of the Abyss have opened. Early access to Aion 2 is now live, bringing sweeping new mechanics, redesigned classes, and breathtaking visuals to the eternal conflict between Elyos and Asmodians.",
                Content = "# Aion 2 Early Access Begins\n\nThe wait is finally over. After years of anticipation, **Aion 2** has officially entered Early Access...\n\nNew mechanics, new classes, and a reimagined Abyss await brave Daevas willing to forge their legend.",
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
                Summary = "NCSoft reveals the devastating Shadow Reaper — a dark melee-ranged hybrid class wielding twin soul-swords. Master the shadows and harvest the life force of your enemies.",
                Content = "# Shadow Reaper — Class Overview\n\nThe **Shadow Reaper** is a fearsome Asmodian-aligned class that blends melee ferocity with dark magic...\n\nExpect high burst damage, stealth mechanics, and a unique soul-harvest resource system.",
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
                Summary = "Scheduled maintenance on Saturday — patch 1.3 brings critical balance changes to PvP combat, Abyss point rewards, and Fortress siege timers across all servers.",
                Content = "# Patch 1.3 — Balance & Maintenance\n\n## Schedule\nMaintenance window: **Saturday 02:00 – 06:00 UTC**\n\n## Highlights\n- Gladiator: Templar Shield Stun duration reduced\n- Ranger: Bow Skills damage increased by 8%\n- Abyss Point rewards increased for Fortress participation",
                ImageUrl = "https://picsum.photos/800/450?random=12",
                PublishedAt = new DateTime(2025, 2, 10, 9, 0, 0, DateTimeKind.Utc),
                AuthorName = "AionOverdose Staff",
                IsPublished = true
            }
        );

        modelBuilder.Entity<PlayerRanking>().HasData(
            new PlayerRanking { Id = 1, PlayerName = "DarkWingDaeva", Class = "Gladiator", Level = 65, Score = 1_250_000, Rank = 1, UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new PlayerRanking { Id = 2, PlayerName = "SkyBlazer", Class = "Ranger", Level = 65, Score = 1_180_500, Rank = 2, UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new PlayerRanking { Id = 3, PlayerName = "AbyssQueen", Class = "Sorcerer", Level = 65, Score = 1_050_200, Rank = 3, UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new PlayerRanking { Id = 4, PlayerName = "IronTemplar", Class = "Templar", Level = 64, Score = 987_300, Rank = 4, UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new PlayerRanking { Id = 5, PlayerName = "StormChaser", Class = "Assassin", Level = 63, Score = 912_750, Rank = 5, UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<WikiEntry>().HasData(
            new WikiEntry { Id = 1, Title = "Getting Started in Aion 2", Slug = "getting-started", Category = "Beginner", Content = "# Getting Started\n\nWelcome to Aion 2! This guide will help new Daevas find their wings...", UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new WikiEntry { Id = 2, Title = "Gladiator Class Guide", Slug = "gladiator-class-guide", Category = "Classes", Content = "# Gladiator\n\nThe Gladiator is the frontline warrior of the Elyos faction...", UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new WikiEntry { Id = 3, Title = "Abyss PvP Guide", Slug = "abyss-pvp-guide", Category = "PvP", Content = "# Abyss PvP Guide\n\nThe Abyss is the central battleground between Elyos, Asmodians, and the Balaur...", UpdatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
