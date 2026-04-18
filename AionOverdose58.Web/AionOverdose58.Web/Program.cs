using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using AionOverdose58.Web.Client.Pages;
using AionOverdose58.Web.Components;
using AionOverdose58.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add HttpContextAccessor for IP address tracking
builder.Services.AddHttpContextAccessor();

// Cascade authentication state to all components
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
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
}

app.UseHttpsRedirection();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AionOverdose58.Web.Client._Imports).Assembly);

app.Run();
