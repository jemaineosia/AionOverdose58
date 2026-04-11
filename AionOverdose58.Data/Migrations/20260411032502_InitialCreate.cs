using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AionOverdose58.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerRankings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Class = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<long>(type: "bigint", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRankings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WikiEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WikiEntries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NewsArticles",
                columns: new[] { "Id", "AuthorName", "Content", "ImageUrl", "IsPublished", "PublishedAt", "Slug", "Summary", "Title" },
                values: new object[,]
                {
                    { 1, "AionOverdose Staff", "# Aion 2 Early Access Begins\n\nThe wait is finally over. After years of anticipation, **Aion 2** has officially entered Early Access...\n\nNew mechanics, new classes, and a reimagined Abyss await brave Daevas willing to forge their legend.", "https://picsum.photos/800/450?random=10", true, new DateTime(2025, 3, 15, 10, 0, 0, 0, DateTimeKind.Utc), "aion2-early-access-begins", "The gates of the Abyss have opened. Early access to Aion 2 is now live, bringing sweeping new mechanics, redesigned classes, and breathtaking visuals to the eternal conflict between Elyos and Asmodians.", "Aion 2 Early Access Begins — The Eternal Sky Awakens" },
                    { 2, "AionOverdose Staff", "# Shadow Reaper — Class Overview\n\nThe **Shadow Reaper** is a fearsome Asmodian-aligned class that blends melee ferocity with dark magic...\n\nExpect high burst damage, stealth mechanics, and a unique soul-harvest resource system.", "https://picsum.photos/800/450?random=11", true, new DateTime(2025, 2, 28, 14, 0, 0, 0, DateTimeKind.Utc), "shadow-reaper-class-reveal", "NCSoft reveals the devastating Shadow Reaper — a dark melee-ranged hybrid class wielding twin soul-swords. Master the shadows and harvest the life force of your enemies.", "New Class Reveal: The Shadow Reaper Joins the Battle" },
                    { 3, "AionOverdose Staff", "# Patch 1.3 — Balance & Maintenance\n\n## Schedule\nMaintenance window: **Saturday 02:00 – 06:00 UTC**\n\n## Highlights\n- Gladiator: Templar Shield Stun duration reduced\n- Ranger: Bow Skills damage increased by 8%\n- Abyss Point rewards increased for Fortress participation", "https://picsum.photos/800/450?random=12", true, new DateTime(2025, 2, 10, 9, 0, 0, 0, DateTimeKind.Utc), "server-maintenance-balance-patch-1-3", "Scheduled maintenance on Saturday — patch 1.3 brings critical balance changes to PvP combat, Abyss point rewards, and Fortress siege timers across all servers.", "Server Maintenance & Balance Patch 1.3 Notes" }
                });

            migrationBuilder.InsertData(
                table: "PlayerRankings",
                columns: new[] { "Id", "Class", "Level", "PlayerName", "Rank", "Score", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Gladiator", 65, "DarkWingDaeva", 1, 1250000L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Ranger", 65, "SkyBlazer", 2, 1180500L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Sorcerer", 65, "AbyssQueen", 3, 1050200L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Templar", 64, "IronTemplar", 4, 987300L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "Assassin", 63, "StormChaser", 5, 912750L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "WikiEntries",
                columns: new[] { "Id", "Category", "Content", "Slug", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Beginner", "# Getting Started\n\nWelcome to Aion 2! This guide will help new Daevas find their wings...", "getting-started", "Getting Started in Aion 2", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Classes", "# Gladiator\n\nThe Gladiator is the frontline warrior of the Elyos faction...", "gladiator-class-guide", "Gladiator Class Guide", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "PvP", "# Abyss PvP Guide\n\nThe Abyss is the central battleground between Elyos, Asmodians, and the Balaur...", "abyss-pvp-guide", "Abyss PvP Guide", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_Slug",
                table: "NewsArticles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Username",
                table: "UserAccounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WikiEntries_Slug",
                table: "WikiEntries",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsArticles");

            migrationBuilder.DropTable(
                name: "PlayerRankings");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "WikiEntries");
        }
    }
}
