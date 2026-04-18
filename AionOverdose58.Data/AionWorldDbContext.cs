using AionOverdose58.Shared.Models.Aion;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Data;

/// <summary>
/// DbContext for the AionWorld game database.
/// READ-ONLY — character and world data for the Aion game server.
/// </summary>
public class AionWorldDbContext : DbContext
{
    public AionWorldDbContext(DbContextOptions<AionWorldDbContext> options) : base(options)
    {
    }

    public DbSet<AionCharacter> Characters => Set<AionCharacter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AionCharacter>(entity =>
        {
            entity.ToTable("user_data");
            entity.HasKey(e => e.CharId);
            entity.Property(e => e.CharId).HasColumnName("char_id");
            entity.Property(e => e.Name).HasColumnName("user_id").HasMaxLength(20);
            entity.Property(e => e.AccountName).HasColumnName("account_name").HasMaxLength(14);
            entity.Property(e => e.Race).HasColumnName("race");
            entity.Property(e => e.Level).HasColumnName("lev");
            entity.Property(e => e.PlayerClass).HasColumnName("class");
            entity.Property(e => e.IsBanned).HasColumnName("is_banned");
            entity.Property(e => e.IsFemale).HasColumnName("gender");
            entity.Property(e => e.GuildId).HasColumnName("guild_id");
            entity.Property(e => e.LastLoginTime).HasColumnName("last_login_time");
            entity.Property(e => e.LastLogoutTime).HasColumnName("last_logout_time");
        });
    }
}
