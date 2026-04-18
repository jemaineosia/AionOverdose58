# MacBook Setup Guide for Aion Overdose 58

This guide will help you quickly get up and running with the Aion Overdose 58 project on your MacBook.

## Prerequisites

1. **.NET 10 SDK** - [Download for macOS](https://dotnet.microsoft.com/download)
2. **Visual Studio Code** or **Visual Studio for Mac**
3. **GitHub Copilot** extension installed
4. **SQL Server** access (remote server at 158.69.54.217)

## Quick Setup Steps

### 1. Clone the Repository
```bash
git clone https://github.com/jemaineosia/AionOverdose58.git
cd AionOverdose58
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Review GitHub Copilot Instructions
The project includes comprehensive context for GitHub Copilot:
- **Location**: `.github/copilot-instructions.md`
- **Purpose**: Provides complete project context, architecture, and coding patterns
- GitHub Copilot will automatically use this file to understand the project

### 4. Review Project Documentation
All documentation is in the `/Documentation` folder:
- `Quick-Reference.md` - Project overview
- `Identity-Integration-Guide.md` - Authentication system
- `Email-Verification-Guide.md` - Email service
- `Password-Reset-Guide.md` - Password reset flow
- `Advanced-Security-Features.md` - Security details

### 5. Configure Connection Strings (Optional)
If working with the database, update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=158.69.54.217;Database=AionGameCP;User Id=sa;Password=4i0nOverD0s3!@;MultipleActiveResultSets=true;TrustServerCertificate=True",
    "AionAccountsConnection": "Server=158.69.54.217;Database=AionAccounts;User Id=sa;Password=4i0nOverD0s3!@;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

> **Note**: Consider using User Secrets for sensitive data:
> ```bash
> dotnet user-secrets init --project AionOverdose58.Web/AionOverdose58.Web
> dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=158.69.54.217;..."
> ```

### 6. Run Migrations (If needed)
```bash
dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
```

### 7. Run the Application
```bash
cd AionOverdose58.Web/AionOverdose58.Web
dotnet run
```

Access at: `https://localhost:5001` or `http://localhost:5000`

## Key Project Information

### Solution Structure
- **AionOverdose58.Web** - Main Blazor server project
- **AionOverdose58.Web.Client** - Blazor WebAssembly client
- **AionOverdose58.Data** - EF Core DbContexts and migrations
- **AionOverdose58.Shared** - Shared models and DTOs
- **AionOverdose58.Tests** - Unit and integration tests

### Technology Stack
- .NET 10
- Blazor Web App (Auto interactivity: SSR + WebAssembly)
- ASP.NET Core Identity with email verification
- SQL Server (2 databases: AionGameCP + AionAccounts)
- Entity Framework Core 10
- Tailwind CSS (CDN for dev)

### Important Coding Patterns

#### Always Use IDbContextFactory
```csharp
@inject IDbContextFactory<AppDbContext> DbFactory

await using var context = await DbFactory.CreateDbContextAsync();
// use context here
```

#### Two Databases
1. **AionGameCP** - Identity + app data (read/write)
2. **AionAccounts** - Legacy Aion database (READ-ONLY except PIN updates)

### Admin Credentials
- **Email**: admin@aionoverdose58.com
- **Password**: Admin123!

> ⚠️ Change password after first login!

## GitHub Copilot Usage

With the `.github/copilot-instructions.md` file in place, GitHub Copilot will understand:
- Project architecture and database structure
- Authentication and security patterns
- Service layer design
- Common coding patterns and gotchas
- Email service configuration
- Development workflow

Simply ask Copilot questions like:
- "How do I add a new Identity page?"
- "Show me how to access the Aion game account"
- "Create a service to manage news articles"
- "How do I send an email?"

## Common Commands

### Entity Framework Migrations
```bash
# Create migration
dotnet ef migrations add MigrationName --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web

# Apply migration
dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
```

### Build and Run
```bash
# Build
dotnet build

# Run (from solution root)
dotnet run --project AionOverdose58.Web/AionOverdose58.Web

# Run (from project directory)
cd AionOverdose58.Web/AionOverdose58.Web
dotnet run
```

### Clean and Rebuild
```bash
dotnet clean
dotnet build
```

## Visual Studio Code Extensions (Recommended)

1. **C# Dev Kit** - C# language support
2. **GitHub Copilot** - AI pair programming
3. **GitHub Copilot Chat** - Chat interface for Copilot
4. **.NET Install Tool** - Manage .NET SDK versions
5. **Tailwind CSS IntelliSense** - Tailwind autocomplete

## Troubleshooting

### "dotnet command not found"
Install .NET SDK for macOS from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)

### Database Connection Issues
- Ensure SQL Server allows remote connections
- Check firewall rules for port 1433
- Verify credentials in connection string

### Email Service Not Working
- Gmail requires App Password (not regular password)
- Check `EmailSettings` in `appsettings.json`
- For testing, use Mailtrap or Papercut SMTP

## Next Steps

1. Review the main `README.md` for comprehensive project overview
2. Read `.github/copilot-instructions.md` for detailed architecture
3. Explore the `/Documentation` folder for guides
4. Start coding with GitHub Copilot assistance!

---

**Happy Coding!** 🚀
