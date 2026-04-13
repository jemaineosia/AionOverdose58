using AionOverdose58.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Article> Articles => Set<Article>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Category).IsRequired()
                  .HasConversion<string>().HasMaxLength(50);
            entity.Property(e => e.PreviewContent).HasMaxLength(1000);
            entity.Property(e => e.Content).IsRequired();   // HTML — no length cap
            entity.Property(e => e.ImageUrl).HasMaxLength(2000);
            entity.Property(e => e.CreatedOn).IsRequired();
            entity.Property(e => e.UpdatedOn).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);
        });
    }
}
