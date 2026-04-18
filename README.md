# Aion Overdose 58

A dark fantasy fansite for **Aion 2**, built with **.NET 10 Blazor Web App (Auto interactivity mode)**, **Tailwind CSS**, and **SQL Server** via Entity Framework Core.

This project includes a complete **ASP.NET Core Identity** implementation with email verification, password reset, and PIN recovery features, integrating seamlessly with a legacy Aion game server database.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | .NET 10 Blazor Web App (Auto — SSR + WASM) |
| Styling | Tailwind CSS (via CDN for dev; PostCSS for production) |
| Fonts | Google Fonts: Cinzel + Raleway |
| Database | SQL Server via Entity Framework Core 10 |
| Authentication | ASP.NET Core Identity with Email Verification |
| Email Service | Gmail SMTP with configurable settings |
| ORM | EF Core with Code-First migrations & seed data |

---

## Solution Structure

```
AionOverdose58/
├── AionOverdose58.slnx                        # Solution file
├── .github/
│   └── copilot-instructions.md                # GitHub Copilot project context
├── Documentation/                             # Comprehensive project documentation
│   ├── Quick-Reference.md
│   ├── Identity-Integration-Guide.md
│   ├── Email-Verification-Guide.md
│   ├── Email-Quick-Setup.md
│   ├── Password-Reset-Guide.md
│   ├── Password-Reset-Quick-Reference.md
│   ├── Security-Quick-Reference.md
│   └── Advanced-Security-Features.md
├── AionOverdose58.Shared/                     # Shared models & DTOs
│   ├── Models/
│   │   ├── ApplicationUser.cs                 # Custom Identity user with Aion account link
│   │   ├── EmailSettings.cs                   # Email configuration model
│   │   ├── NewsArticle.cs
│   │   ├── WikiEntry.cs
│   │   ├── PlayerRanking.cs
│   │   └── UserAccount.cs
│   └── DTOs/
│       ├── NewsArticleDto.cs
│       ├── WikiEntryDto.cs
│       ├── PlayerRankingDto.cs
│       └── UserAccountDto.cs
├── AionOverdose58.Data/                       # EF Core + SQL Server
│   ├── AppDbContext.cs                        # Main database (Identity + App tables)
│   ├── AionAccountsDbContext.cs               # Legacy Aion database (READ-ONLY)
│   └── Migrations/
├── AionOverdose58.Tests/                      # Unit & integration tests
│   └── AionOverdose58.Tests.csproj
├── AionOverdose58.Web/
│   ├── AionOverdose58.Web/                    # Blazor Server (SSR + Interactive Server)
│   │   ├── Components/
│   │   │   ├── App.razor                      # Root with Tailwind CDN + Google Fonts
│   │   │   ├── Routes.razor
│   │   │   ├── Layout/
│   │   │   │   ├── MainLayout.razor
│   │   │   │   ├── NavBar.razor               # Logo, nav links, Account dropdown
│   │   │   │   └── SocialBar.razor            # Facebook, YouTube, Discord
│   │   │   ├── Pages/
│   │   │   │   ├── Home.razor                 # Hero slider + Latest News
│   │   │   │   ├── News.razor                 # News grid with pagination
│   │   │   │   ├── NewsDetail.razor           # Single article view
│   │   │   │   ├── Ranking.razor              # Player rankings table
│   │   │   │   ├── NotFound.razor             # 404 page
│   │   │   │   └── Account/
│   │   │   │       ├── Login.razor            # Login with email confirmation check
│   │   │   │       ├── Register.razor         # Registration with email verification
│   │   │   │       ├── ConfirmEmail.razor     # Email confirmation handler
│   │   │   │       ├── ResendConfirmation.razor # Resend confirmation email
│   │   │   │       ├── ForgotPassword.razor   # Password reset request
│   │   │   │       ├── ResetPassword.razor    # Password reset with token
│   │   │   │       ├── PinRecovery.razor      # PIN recovery for Aion accounts
│   │   │   │       └── ControlPanel.razor     # User dashboard (WIP)
│   │   │   └── Shared/
│   │   │       └── HeroSlider.razor           # Full-screen auto-sliding hero
│   │   ├── Services/
│   │   │   ├── NewsService.cs
│   │   │   ├── RankingService.cs
│   │   │   ├── EmailService.cs                # SMTP email service
│   │   │   ├── AccountService.cs              # Aion account integration
│   │   │   └── RoleSeeder.cs                  # Admin role/user initialization
│   │   ├── Program.cs                         # Service registration & middleware
│   │   ├── appsettings.json                   # Configuration (DB, Email, etc.)
│   │   └── wwwroot/
│   │       └── app.css                        # Custom scrollbar + minimal global CSS
│   └── AionOverdose58.Web.Client/             # Blazor WASM (Auto interactivity client)
│       └── Program.cs
```

---

## Key Features

### 🔐 Authentication & Security
- **ASP.NET Core Identity** with custom user model
- **Email Verification** - Users must confirm email before login
- **Password Reset** - Secure token-based password recovery
- **PIN Recovery** - Links to legacy Aion game accounts
- **Account Lockout** - Protection against brute force attacks (5 attempts, 15 min lockout)
- **Admin Role Seeding** - Auto-creates admin user on first run

### 🎨 Design & UI
- 🌑 **Dark gothic/fantasy design** with full-viewport background
- 🎠 **Hero Slider** - Full-screen, auto-advancing with arrows and dot indicators
- **Responsive Layout** - Mobile hamburger menu, responsive grid layouts
- **Custom Fonts** - Cinzel (headings) + Raleway (body) from Google Fonts
- **Custom Scrollbar** - Themed to match dark aesthetic

### 📰 Content Management
- **News System** - Paginated list and detail pages, DB-backed with seed data
- 🏆 **Player Rankings** - Gold/silver/bronze highlights, DB-backed
- **Wiki System** - (WIP) Knowledge base for game information
- 📱 **Social Integration** - Fixed Facebook, YouTube, Discord buttons

### 🗃️ Database Architecture
- **Dual Database Approach**:
  - `AionGameCP` - Identity tables + application data
  - `AionAccounts` - Legacy Aion game server database (READ-ONLY)
- **Entity Framework Core 10** with Code-First migrations
- **IDbContextFactory Pattern** - Proper DbContext lifecycle in Blazor

### 📧 Email Service
- **Gmail SMTP Integration** - Configurable via appsettings.json
- **Email Templates** - Branded emails for verification, password reset, etc.
- **Queue-Ready** - Easy to integrate with background job processors

---

## Quick Start

### Prerequisites
- **.NET 10 SDK** (or later)
- **SQL Server** (local, remote, or Azure)
- **Gmail Account** with App Password (for email service)

### 1. Clone & Restore
```bash
git clone https://github.com/jemaineosia/AionOverdose58.git
cd AionOverdose58
dotnet restore
```

### 2. Configure Database
Update connection strings in `AionOverdose58.Web/AionOverdose58.Web/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=AionGameCP;User Id=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=True",
    "AionAccountsConnection": "Server=YOUR_SERVER;Database=AionAccounts;User Id=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

### 3. Configure Email Service
Update email settings in `appsettings.json`:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderName": "Aion Overdose 58",
    "SenderEmail": "noreply@aionoverdose58.com",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "EnableSsl": true
  }
}
```

> **Security Note**: For production, move sensitive settings to **Azure Key Vault** or **User Secrets**.

### 4. Run Database Migrations
```bash
dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
```
This will:
- Create the `AionGameCP` database
- Set up ASP.NET Core Identity tables
- Seed sample data (news articles, rankings, wiki entries)
- Create admin user (email: admin@aionoverdose58.com, password: Admin123!)

### 5. Run the Application
```bash
cd AionOverdose58.Web/AionOverdose58.Web
dotnet run
```
Open `https://localhost:5001` or `http://localhost:5000`.

### 6. First Login
Use the default admin account:
- **Email**: `admin@aionoverdose58.com`
- **Password**: `Admin123!`

> ⚠️ **Important**: Change the admin password immediately after first login!

---

## Documentation

Comprehensive documentation is available in the `/Documentation` folder:

### Quick References
- [Quick Reference](Documentation/Quick-Reference.md) - Overall project overview
- [Email Quick Setup](Documentation/Email-Quick-Setup.md) - Email service configuration
- [Password Reset Quick Reference](Documentation/Password-Reset-Quick-Reference.md) - Password reset flow
- [Security Quick Reference](Documentation/Security-Quick-Reference.md) - Security features overview

### Detailed Guides
- [Identity Integration Guide](Documentation/Identity-Integration-Guide.md) - ASP.NET Core Identity implementation
- [Email Verification Guide](Documentation/Email-Verification-Guide.md) - Email verification system
- [Password Reset Guide](Documentation/Password-Reset-Guide.md) - Password reset implementation
- [Advanced Security Features](Documentation/Advanced-Security-Features.md) - Security deep dive

### GitHub Copilot Instructions
- [Copilot Instructions](.github/copilot-instructions.md) - Complete project context for AI assistance

---

## Development Workflow

### Running Migrations
```bash
# Create a new migration
dotnet ef migrations add MigrationName --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web

# Apply migrations to database
dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
```

### Testing Email Service
For development, consider using:
- [Mailtrap](https://mailtrap.io/) - Email testing service
- [Papercut SMTP](https://github.com/ChangemakerStudios/Papercut-SMTP) - Local SMTP server

### Building for Production
```bash
dotnet publish -c Release
```

---

## Important Coding Patterns

### DbContext Usage in Blazor
**Always use IDbContextFactory** - Never inject DbContext directly:

```csharp
@inject IDbContextFactory<AppDbContext> DbFactory

@code {
    protected override async Task OnInitializedAsync()
    {
        await using var context = await DbFactory.CreateDbContextAsync();
        // Use context here
    }
}
```

### Accessing Aion Game Account
```csharp
@inject AccountService AccountService

var aionAccount = await AccountService.GetAionAccountAsync(accountName);
if (aionAccount != null)
{
    // Read account data
    // NEVER write directly except for PIN updates
}
```

---

## Production Deployment

### Tailwind CSS with PostCSS

The current setup uses the Tailwind CDN for development. For production, switch to a PostCSS build:

1. Install Node.js dependencies:
```bash
cd AionOverdose58.Web/AionOverdose58.Web
npm init -y
npm install -D tailwindcss @tailwindcss/typography postcss autoprefixer
npx tailwindcss init
```

2. Create `tailwind.config.js` with content paths pointing to your `.razor` files.
3. Create `postcss.config.js`.
4. Add a build script and remove the CDN script tag from `App.razor`.

### Security Hardening for Production

1. **Move Secrets** - Use Azure Key Vault or User Secrets:
   ```bash
   dotnet user-secrets init --project AionOverdose58.Web/AionOverdose58.Web
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
   dotnet user-secrets set "EmailSettings:Password" "YOUR_EMAIL_PASSWORD"
   ```

2. **Enable HTTPS** - Enforce HTTPS redirection (already configured in Program.cs)

3. **Add Rate Limiting** - Prevent abuse of password reset/email endpoints

4. **Implement CAPTCHA** - Add reCAPTCHA to registration and login forms

5. **Enable Logging** - Configure Application Insights or Serilog for production logging

6. **Database Backups** - Set up automated backups for SQL Server

### Environment Variables (Azure App Service)

```
ConnectionStrings__DefaultConnection=Server=...
ConnectionStrings__AionAccountsConnection=Server=...
EmailSettings__Username=your-email@gmail.com
EmailSettings__Password=your-app-password
```

---

## Design Theme & Assets

### Color Palette (Dark Fantasy Gothic)

| Token | Hex | Usage |
|-------|-----|-------|
| `aion-dark` | `#080b14` | Primary background |
| `aion-red` | `#8b1a1a` | Accents and borders |
| `aion-crimson` | `#c0392b` | Highlights and CTAs |
| `aion-gold` | `#c9a84c` | Premium/important elements |
| `aion-teal` | `#1a4a4a` | Alternate accents |

### Typography
| Element | Font | Source |
|---------|------|--------|
| Headings | **Cinzel** (serif) | Google Fonts - Medieval/fantasy feel |
| Body | **Raleway** (sans-serif) | Google Fonts - Clean, readable |

### Replacing Placeholder Images

Placeholder images (`https://picsum.photos/...`) are used in:
- `HeroSlider.razor` - Replace with real Aion 2 artwork URLs
- Seed data in `AppDbContext.cs` - Update `ImageUrl` fields with real CDN URLs

Example Aion 2 CDN URLs (NCSoft official assets):
```
https://aion2.plaync.com/...
```

> ⚠️ **Copyright Notice**: Be mindful of NCSoft's IP/copyright when using official game assets.

---

## Project Architecture Highlights

### Two Database Approach
1. **AionGameCP** - Identity + Application data (full read/write)
2. **AionAccounts** - Legacy game server database (READ-ONLY except PIN updates)

### Service Layer
- **AccountService** - Manages integration between Identity users and Aion game accounts
- **EmailService** - Handles all email communications (verification, password reset)
- **RoleSeeder** - Initializes admin role and user on startup

### Identity Features
- Custom `ApplicationUser` model with additional properties
- Email verification required before login
- Password reset with secure tokens
- PIN recovery for Aion accounts
- Account lockout protection

---

## Common Gotchas & Troubleshooting

1. **Email Not Sending**
   - Check SMTP credentials in appsettings.json
   - Use Gmail App Password, not regular password
   - Verify "Less secure app access" is enabled (if required)

2. **Token Expired**
   - Email confirmation tokens: 3 days
   - Password reset tokens: 3 hours
   - Users must use links before expiration

3. **Database Connection Issues**
   - Verify SQL Server is running
   - Check connection string format
   - Confirm firewall allows connection (port 1433)
   - `TrustServerCertificate=True` is for development only

4. **AionAccounts Database**
   - READ-ONLY except for PIN updates
   - Never modify account_data directly (use AccountService)

5. **Circular Dependencies**
   - Always use `IDbContextFactory<AppDbContext>` in Blazor components
   - Never inject `DbContext` directly

---

## Future Enhancements

### Planned Features
- [ ] User profile management page
- [ ] Two-factor authentication (2FA)
- [ ] Admin panel for content management
- [ ] Forum/community features
- [ ] Character management (link to Aion database)
- [ ] News article creation/editing (admin only)
- [ ] Wiki editing system with WYSIWYG editor
- [ ] Donation/payment integration
- [ ] Server status monitoring
- [ ] Discord bot integration
- [ ] Real-time notifications (SignalR)
- [ ] Item database and calculator
- [ ] Event calendar

### Technical Improvements
- [ ] Migrate from Tailwind CDN to PostCSS build
- [ ] Add comprehensive unit tests
- [ ] Implement integration tests
- [ ] Add API rate limiting
- [ ] Implement CAPTCHA for forms
- [ ] Add audit logging for admin actions
- [ ] Optimize database queries with caching
- [ ] Add localization support (i18n)

---

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Follow existing code style and patterns
4. Use `IDbContextFactory` for database access
5. Write tests for new features
6. Commit your changes (`git commit -m 'Add amazing feature'`)
7. Push to the branch (`git push origin feature/amazing-feature`)
8. Open a Pull Request

### Code Style
- Use `var` for local variables when type is obvious
- Async all the way (no sync-over-async)
- Keep Razor components focused (separate logic into services)
- Follow existing naming conventions
- Add XML documentation for public APIs

---

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## Acknowledgments

- **NCsoft** - For creating the Aion universe
- **.NET Team** - For Blazor and ASP.NET Core
- **Tailwind CSS** - For the utility-first CSS framework
- **Google Fonts** - For Cinzel and Raleway typefaces

---

## Support & Contact

- **Documentation**: See `/Documentation` folder
- **Issues**: [GitHub Issues](https://github.com/jemaineosia/AionOverdose58/issues)
- **Repository**: [github.com/jemaineosia/AionOverdose58](https://github.com/jemaineosia/AionOverdose58)

---

**Last Updated**: January 2025 - .NET 10 with complete Identity implementation
