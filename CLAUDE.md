# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Git Workflow

After completing any meaningful unit of work, commit and push to GitHub immediately. Never accumulate large batches of unrelated changes in a single commit.

- Use clear, specific commit messages that describe what changed and why (e.g. `fix login redirect after email confirmation`, not `fix stuff`).
- Push to the remote branch after each commit so work is never at risk of being lost.
- Prefer small, focused commits over large ones — one logical change per commit.

## Commands

```bash
# Restore & build
dotnet restore
dotnet build

# Run the web app
cd AionOverdose58.Web/AionOverdose58.Web && dotnet run

# Run tests
dotnet test
dotnet test AionOverdose58.Tests

# EF Core migrations
dotnet ef migrations add <Name> --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
```

## Architecture

This is a **dark-fantasy fansite for Aion 2** built as a Blazor Web App (Auto render mode: SSR + WebAssembly) on .NET 10.

### Solution Projects

| Project | Role |
|---------|------|
| `AionOverdose58.Web` | Main server-side app — Razor components, services, middleware, seeding |
| `AionOverdose58.Web.Client` | WASM client components (interactive client-side rendering) |
| `AionOverdose58.Data` | EF Core DbContexts and migrations |
| `AionOverdose58.Shared` | DTOs, models, interfaces shared across all projects |
| `AionOverdose58.Tests` | xUnit + Moq unit tests |

### Databases (Three Separate Connections)

1. **`DefaultConnection` → `AionGameCP`** — Primary app DB: Identity tables, Articles, UserAccounts, AppLogs. Read/write.
2. **`AionAccountsConnection` → `AionAccounts`** — Legacy game server accounts (`account_data`, `account_time`). Treat as read-only except PIN updates via `AccountService`.
3. **`AionWorldConnection` → `AionWorld`** — Game world data (characters, items). Read-only.

Migrations live in `AionOverdose58.Data/Migrations/` and target `DefaultConnection` only. The game databases are managed externally.

### Service Layer Pattern

All business logic lives in `AionOverdose58.Web/Services/`. Key services:

- **`AccountService`** — Links ASP.NET Identity users to Aion game accounts; wraps both `AionGameCP` and `AionAccounts` DBs.
- **`EmailService`** — Gmail SMTP via MailKit; sends verification emails and password-reset links.
- **`ArticleService` / `ArticleReadService`** — News CRUD and paginated reads (split write/read responsibilities).
- **`RankingService`** — Reads character rankings from `AionWorldConnection`.
- **`RecaptchaService`** — Google reCAPTCHA v3 verification for all public forms.
- **`AuditLogger`** — Tracks sensitive operations (account link, PIN change, etc.).

### DbContext Pattern

Use `IDbContextFactory<T>` (not direct `DbContext` injection) in Blazor components and services to manage DbContext lifetime correctly across render modes. The three factories are:

- `IDbContextFactory<ApplicationDbContext>` — app DB
- `IDbContextFactory<AionAccountsDbContext>` — game accounts DB
- `IDbContextFactory<AionWorldDbContext>` — game world DB

### Authentication Flow

ASP.NET Core Identity with **email verification required before login**. Flow: Register → receive email → confirm → login. Password reset and PIN recovery also go through email. Account lockout is configured (5 attempts, 15-minute lockout).

### Startup Seeding

`RoleSeeder` and `ArticleSeeder` run at app startup in `Program.cs`. They are idempotent — safe to run on every start.

### Middleware

`RequestLoggingMiddleware` intercepts all requests and logs username + IP to Serilog (persisted to SQL Server `AppLogs` table and rolling daily files under `logs/`).

### UI / Styling

Tailwind CSS (CDN in development). Custom dark-gothic theme colors defined in `tailwind.config.js`:
- `aion-dark` (#080b14), `aion-red` (#8b1a1a), `aion-crimson` (#c0392b), `aion-gold` (#c9a84c), `aion-teal` (#1a4a4a)

Pages are in `AionOverdose58.Web/Components/Pages/`. Shared layout components (NavBar, HeroSlider, SocialBar, Footer) are in `Components/Layout/`.

## Configuration

`appsettings.json` requires these sections to be populated locally (not committed with real values):
- `ConnectionStrings` — three SQL Server connection strings
- `EmailSettings` — Gmail SMTP host, port, sender address, and App Password
- `RecaptchaSettings` — Google reCAPTCHA v3 `SiteKey` and `SecretKey`

Serilog is configured in `appsettings.json` under `"Serilog"` with two sinks: SQL Server (`AppLogs` table) and rolling file (`logs/app-{date}.log`).
