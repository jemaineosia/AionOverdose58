using AionOverdose58.Shared.Models.Aion;
using Microsoft.EntityFrameworkCore;

namespace AionOverdose58.Data;

/// <summary>
/// DbContext for the AionAccounts database.
/// Handles user accounts and authentication data for the Aion game server.
/// </summary>
public class AionAccountsDbContext : DbContext
{
    public AionAccountsDbContext(DbContextOptions<AionAccountsDbContext> options) : base(options)
    {
    }

    // Tables used by stored procedure od_CreateAccount
    public DbSet<AionSsn> Ssns => Set<AionSsn>();
    public DbSet<AionUserAccount> UserAccounts => Set<AionUserAccount>();
    public DbSet<AionUserAuth> UserAuths => Set<AionUserAuth>();
    public DbSet<AionUserInfo> UserInfos => Set<AionUserInfo>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure SSN table (used by stored procedure)
        modelBuilder.Entity<AionSsn>(entity =>
        {
            entity.ToTable("ssn");
            entity.HasKey(e => e.Ssn);
            entity.Property(e => e.Ssn).HasColumnName("ssn").HasMaxLength(13).IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(50);
            entity.Property(e => e.Newsletter).HasColumnName("newsletter").HasDefaultValue(0);
            entity.Property(e => e.Job).HasColumnName("job").HasDefaultValue(0);
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.Mobile).HasColumnName("mobile").HasMaxLength(20);
            entity.Property(e => e.RegDate).HasColumnName("reg_date");
            entity.Property(e => e.Zip).HasColumnName("zip").HasMaxLength(10);
            entity.Property(e => e.AddrMain).HasColumnName("addr_main").HasMaxLength(255);
            entity.Property(e => e.AddrEtc).HasColumnName("addr_etc").HasMaxLength(255);
            entity.Property(e => e.AccountNum).HasColumnName("account_num");
            entity.Property(e => e.StatusFlag).HasColumnName("status_flag").HasDefaultValue(0);
        });

        // Configure user_account table (used by stored procedure)
        modelBuilder.Entity<AionUserAccount>(entity =>
        {
            entity.ToTable("user_account");
            entity.HasKey(e => e.Uid);
            entity.Property(e => e.Uid).HasColumnName("uid").ValueGeneratedOnAdd();
            entity.Property(e => e.Account).HasColumnName("account").HasMaxLength(14).IsRequired();
            entity.Property(e => e.PayStat).HasColumnName("pay_stat").HasDefaultValue(1);
            entity.HasIndex(e => e.Account).IsUnique();
        });

        // Configure user_auth table (used by stored procedure)
        modelBuilder.Entity<AionUserAuth>(entity =>
        {
            entity.ToTable("user_auth");
            entity.HasKey(e => e.Account);
            entity.Property(e => e.Account).HasColumnName("account").HasMaxLength(14).IsRequired();
            entity.Property(e => e.Password).HasColumnName("password").IsRequired();
            entity.Property(e => e.Quiz1).HasColumnName("quiz1").HasMaxLength(255);
            entity.Property(e => e.Quiz2).HasColumnName("quiz2").HasMaxLength(255);
            entity.Property(e => e.Answer1).HasColumnName("answer1");
            entity.Property(e => e.Answer2).HasColumnName("answer2");
            entity.Property(e => e.NewPwdFlag).HasColumnName("new_pwd_flag").HasDefaultValue(0);
            entity.Property(e => e.Passwd).HasColumnName("passwd").HasMaxLength(32);
            entity.Property(e => e.WebPassword).HasColumnName("web_password").HasMaxLength(255);
            entity.Property(e => e.WebLevel).HasColumnName("web_level").HasDefaultValue(0);
        });

        // Configure user_info table (used by stored procedure)
        modelBuilder.Entity<AionUserInfo>(entity =>
        {
            entity.ToTable("user_info");
            entity.HasKey(e => e.Account);
            entity.Property(e => e.Account).HasColumnName("account").HasMaxLength(14).IsRequired();
            entity.Property(e => e.CreateDate).HasColumnName("create_date");
            entity.Property(e => e.Ssn).HasColumnName("ssn").HasMaxLength(13);
            entity.Property(e => e.StatusFlag).HasColumnName("status_flag").HasDefaultValue(0);
            entity.Property(e => e.Kind).HasColumnName("kind").HasDefaultValue(99);
        });

    }
}
