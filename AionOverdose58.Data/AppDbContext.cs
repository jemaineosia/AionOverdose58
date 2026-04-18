using AionOverdose58.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Data;

/// <summary>
/// Application DbContext with ASP.NET Core Identity support.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Article> Articles => Set<Article>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<AppLog> AppLogs => Set<AppLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // IMPORTANT: Call base for Identity tables

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Category).IsRequired()
                  .HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.PreviewContent).HasMaxLength(1000);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.ImageUrl).HasMaxLength(2000);
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.UpdatedOn).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50).HasDefaultValue("Player");
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure ApplicationUser additional properties
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.PinCode).HasMaxLength(6);
            entity.Property(e => e.RegisteredDate).IsRequired();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEmailVerified).HasDefaultValue(false);
        });

        // AppLogs — table is auto-created by Serilog, EF is read-only
        modelBuilder.Entity<AppLog>(entity =>
        {
            entity.ToTable("AppLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Message).HasColumnName("Message");
            entity.Property(e => e.Level).HasColumnName("Level");
            entity.Property(e => e.TimeStamp).HasColumnName("TimeStamp");
            entity.Property(e => e.Exception).HasColumnName("Exception");
            entity.Property(e => e.Username).HasColumnName("Username");
            entity.Property(e => e.IpAddress).HasColumnName("IpAddress");
            entity.Property(e => e.Path).HasColumnName("Path");
        });
    }
}
