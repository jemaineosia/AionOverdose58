using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using AionOverdose58.Web.Client.Pages;
using AionOverdose58.Web.Components;
using AionOverdose58.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections.ObjectModel;
using System.Data;

// ── Bootstrap logger (captures startup errors before full config loads) ──
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// ── Serilog: structured logging to SQL Server (AionGameCP) + rolling file ──
var logConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
// Add a short connect timeout so a slow/unavailable DB never blocks startup
var logConnWithTimeout = logConnectionString.TrimEnd(';') + ";Connect Timeout=5;";

var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties);   // remove XML blob we don't need
columnOptions.Store.Add(StandardColumn.LogEvent);        // add JSON instead
columnOptions.LogEvent.DataLength = 4000;
columnOptions.TimeStamp.NonClusteredIndex = true;
columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn { ColumnName = "Username",  DataType = SqlDbType.NVarChar, DataLength = 256, AllowNull = true },
    new SqlColumn { ColumnName = "IpAddress", DataType = SqlDbType.NVarChar, DataLength = 45,  AllowNull = true },
    new SqlColumn { ColumnName = "Path",      DataType = SqlDbType.NVarChar, DataLength = 1000, AllowNull = true },
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        restrictedToMinimumLevel: LogEventLevel.Information,
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/app-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        restrictedToMinimumLevel: LogEventLevel.Warning,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.MSSqlServer(
        connectionString: logConnWithTimeout,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "AppLogs",
            AutoCreateSqlTable = true,
            BatchPostingLimit = 50,
            BatchPeriod = TimeSpan.FromSeconds(5)
        },
        restrictedToMinimumLevel: LogEventLevel.Information,
        columnOptions: columnOptions)
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// EF Core with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=localhost;Database=AionOverdose58;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// AionAccounts Database Connection
var accountsConnectionString = builder.Configuration.GetConnectionString("AionAccountsConnection")
    ?? "Server=localhost;Database=AionAccounts;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

builder.Services.AddDbContextFactory<AionAccountsDbContext>(options =>
    options.UseSqlServer(accountsConnectionString));

// AionWorld Game Database Connection (read-only)
var aionWorldConnectionString = builder.Configuration.GetConnectionString("AionWorldConnection")
    ?? "Server=localhost;Database=AionWorld;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

builder.Services.AddDbContextFactory<AionWorldDbContext>(options =>
    options.UseSqlServer(aionWorldConnectionString));

// Email Settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Account Settings
builder.Services.Configure<AccountSettings>(builder.Configuration.GetSection("AccountSettings"));
var requireEmailConfirmation = builder.Configuration.GetValue<bool>("AccountSettings:RequireEmailConfirmation", true);

// ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

    // Sign-in settings - controlled by AccountSettings:RequireEmailConfirmation
    options.SignIn.RequireConfirmedEmail = requireEmailConfirmation;
    options.SignIn.RequireConfirmedAccount = requireEmailConfirmation;

    // Token providers
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Token lifespan configuration
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24); // Email confirmation valid for 24 hours
});

// Separate token lifespan for password reset (1 hour for security)
builder.Services.Configure<PasswordHasherOptions>(options =>
{
    options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
    options.IterationCount = 10000;
});

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/account/access-denied";
    options.SlidingExpiration = true;
});

// Application services
builder.Services.AddScoped<IArticleReadService, ArticleReadService>();
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuditLogger, AuditLogger>();
builder.Services.AddScoped<IEmailService, EmailService>();

// reCAPTCHA v3
builder.Services.Configure<RecaptchaSettings>(builder.Configuration.GetSection("RecaptchaSettings"));
builder.Services.AddHttpClient("recaptcha");
builder.Services.AddScoped<IRecaptchaService, RecaptchaService>();

// Add HttpContextAccessor for IP address tracking
builder.Services.AddHttpContextAccessor();

// Cascade authentication state to all components
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Seed roles, admin user, and articles
using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
    await ArticleSeeder.SeedArticlesAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Log every page request with username + IP
app.UseMiddleware<AionOverdose58.Web.Services.RequestLoggingMiddleware>();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AionOverdose58.Web.Client._Imports).Assembly);

try
{
    Log.Information("Application starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
