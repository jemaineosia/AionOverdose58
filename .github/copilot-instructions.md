# GitHub Copilot Instructions for Aion Overdose 58

## Project Overview

This is **Aion Overdose 58**, a dark fantasy fansite for Aion 2, built with .NET 10 Blazor Web App using Auto interactivity mode (SSR + WebAssembly). The project integrates with two SQL Server databases and includes a complete ASP.NET Core Identity system with email verification.

## Technology Stack

- **.NET 10** (targeting net10.0)
- **Blazor Web App** (Auto interactivity: SSR + WebAssembly)
- **Tailwind CSS** (via CDN for development)
- **ASP.NET Core Identity** with email verification
- **SQL Server** (two databases: AionGameCP and AionAccounts)
- **Entity Framework Core 10**

## Solution Architecture

### Projects
1. **AionOverdose58.Web** - Main Blazor server-side project
2. **AionOverdose58.Web.Client** - Blazor WebAssembly client project
3. **AionOverdose58.Data** - Entity Framework Core DbContexts and migrations
4. **AionOverdose58.Shared** - Shared models, DTOs, and interfaces
5. **AionOverdose58.Tests** - Unit and integration tests

## Database Architecture

### Two Database Approach
The application uses **two separate SQL Server databases**:

1. **AionGameCP** (DefaultConnection)
   - ASP.NET Core Identity tables
   - Application-specific tables (News, Wiki, Rankings, etc.)
   - Managed by AppDbContext

2. **AionAccounts** (AionAccountsConnection)
   - Legacy Aion game server database
   - Contains account_data and account_time tables
   - **READ-ONLY** - Never write to this database
   - Accessed via AionAccountsDbContext

### Connection Strings Location
- Development: `appsettings.json`
- Production: Environment variables or Azure Key Vault
- Server: `158.69.54.217`
- Credentials: See appsettings.json (should be moved to user secrets)

## Identity & Authentication System

### Custom ApplicationUser Model
- Extends `IdentityUser`
- Additional properties:
  - `AionAccountName` (links to legacy Aion account)
  - `PinQuestion`, `PinAnswer` (for PIN recovery)
  - `DateOfBirth` (for age verification)
  - Navigation property to `account_data` in AionAccounts DB

### Authentication Features
1. **Email Verification** (required)
   - Users must confirm email before login
   - Resend confirmation email available
   - Confirmation tokens expire (default: 3 days)

2. **Password Reset**
   - Forgot password flow with email tokens
   - Token expiration: default 3 hours
   - Secure password reset page

3. **PIN Recovery**
   - Security question/answer system
   - Links Identity user to Aion game account
   - Updates PIN in AionAccounts database (account_data table)

4. **Account Lockout**
   - Max failed attempts: 5
   - Lockout duration: 15 minutes

### Password Policy
- Minimum length: 6 characters
- Requires digit: Yes
- Requires lowercase: Yes
- Requires uppercase: No
- Requires non-alphanumeric: No
- Unique characters: 1

## Email Service

### Configuration
- Provider: Gmail SMTP
- Settings location: `appsettings.json` → EmailSettings
- Service: `EmailService.cs` (implements `IEmailService`)
- Registration: Registered as Scoped service

### Email Templates
- Confirmation email: Account/ConfirmEmail.razor
- Password reset: Account/ResetPassword.razor
- Customizable sender name and email

## Services Architecture

### AccountService
- **Purpose**: Manages user accounts and integration with Aion game database
- **Key Methods**:
  - `GetAionAccountAsync(accountName)` - Read from AionAccounts DB
  - `UpdatePinAsync(accountName, newPin)` - Update PIN in game database
  - `IsAionAccountLinkedAsync(accountName)` - Check if account is already linked
  - `GetUserByAionAccountAsync(accountName)` - Find Identity user by game account

### EmailService
- **Purpose**: Send emails (verification, password reset, etc.)
- **Methods**:
  - `SendEmailAsync(to, subject, htmlBody)`
- **Configuration**: Uses EmailSettings from appsettings.json

### RoleSeeder
- **Purpose**: Initialize admin role and admin user on startup
- **Admin Account**:
  - Email: admin@aionoverdose58.com
  - Password: Admin123! (should be changed immediately)
  - Role: Administrator
- **Runs**: On application startup in Program.cs

## Important Coding Patterns

### DbContext Usage
- **ALWAYS use IDbContextFactory** - Never inject DbContext directly
- Pattern:
  ```csharp
  @inject IDbContextFactory<AppDbContext> DbFactory
  
  await using var context = await DbFactory.CreateDbContextAsync();
  // use context here
  ```
- Reason: Blazor components are long-lived, DbContext should be short-lived

### Navigation in Blazor
- Use `NavigationManager.NavigateTo()` for redirects
- Use `NavigationManager.ToAbsoluteUri()` for callback URLs in emails
- Example: Email confirmation links must be absolute URLs

### Component Interactivity
- Server-side pages: No `@rendermode` needed (SSR by default)
- Client-side pages: Use `@rendermode InteractiveWebAssembly`
- Auto mode: Use `@rendermode InteractiveAuto`

## Security Best Practices

### Implemented
1. **Email confirmation required** - Users cannot log in without confirming email
2. **Account lockout** - Prevents brute force attacks
3. **Secure token generation** - Uses Data Protection API
4. **Password hashing** - Uses Identity's built-in password hasher
5. **TrustServerCertificate** - Only for development (SQL Server connection)

### TODO (Production)
1. Move connection strings to Azure Key Vault or User Secrets
2. Enable HTTPS enforcement
3. Add rate limiting for password reset/email confirmation
4. Implement CAPTCHA for registration/login
5. Add audit logging for sensitive operations
6. Review and harden CORS policies

## Common Tasks

### Adding a New Identity Page
1. Create .razor file in Components/Pages/Account/
2. Inject required services (UserManager, SignInManager, EmailService, etc.)
3. Use IDbContextFactory pattern for database access
4. Handle form validation with EditForm and DataAnnotationsValidator
5. Display error messages using ValidationSummary
6. Redirect after successful operation using NavigationManager

### Modifying ApplicationUser
1. Add property to `ApplicationUser.cs` in Shared project
2. Create migration:
   ```bash
   dotnet ef migrations add AddPropertyToUser --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
   ```
3. Update database:
   ```bash
   dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
   ```

### Accessing Aion Game Account
1. Get account name from ApplicationUser.AionAccountName
2. Use AccountService to read from AionAccounts DB
3. NEVER write directly to AionAccounts DB except for PIN updates
4. Always check if account exists before linking

## Testing Strategy

### Unit Tests
- Location: AionOverdose58.Tests project
- Framework: xUnit
- Mocking: Moq (for UserManager, DbContext, etc.)

### Integration Tests
- Test database required
- Use TestServer for full stack testing
- Test email sending in isolation

## Documentation

Comprehensive documentation is available in the `/Documentation` folder:

### Quick References
- `Quick-Reference.md` - Overall project quick start
- `Email-Quick-Setup.md` - Email service setup
- `Password-Reset-Quick-Reference.md` - Password reset flow
- `Security-Quick-Reference.md` - Security features overview

### Detailed Guides
- `Identity-Integration-Guide.md` - ASP.NET Core Identity setup
- `Email-Verification-Guide.md` - Email verification implementation
- `Password-Reset-Guide.md` - Password reset implementation
- `Advanced-Security-Features.md` - Security deep dive

## Design & Styling

### Color Scheme (Dark Fantasy Gothic)
- `aion-dark`: `#080b14` (primary background)
- `aion-red`: `#8b1a1a` (accents)
- `aion-crimson`: `#c0392b` (highlights)
- `aion-gold`: `#c9a84c` (premium/important elements)
- `aion-teal`: `#1a4a4a` (alternate accents)

### Fonts
- **Headings**: Cinzel (serif, Google Fonts) - medieval/fantasy feel
- **Body**: Raleway (sans-serif, Google Fonts) - clean, readable

### Tailwind Usage
- Current: CDN (development)
- Production: Should migrate to PostCSS build process
- Custom CSS: `wwwroot/app.css` for scrollbar and global overrides

## Common Gotchas

1. **Email Not Sending**: Check SMTP credentials and app password (not regular Gmail password)
2. **Token Expired**: Tokens expire (3 days for email, 3 hours for password reset)
3. **Circular Dependencies**: Use IDbContextFactory to avoid circular reference issues
4. **AionAccounts DB**: READ-ONLY except for PIN updates
5. **Render Mode**: Must match between layout and page for proper interactivity
6. **UserManager in Client**: Cannot use UserManager in WebAssembly components

## Development Workflow

1. **Running Migrations**:
   ```bash
   dotnet ef migrations add MigrationName --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
   dotnet ef database update --project AionOverdose58.Data --startup-project AionOverdose58.Web/AionOverdose58.Web
   ```

2. **Running the App**:
   ```bash
   cd AionOverdose58.Web/AionOverdose58.Web
   dotnet run
   ```

3. **Building for Production**:
   ```bash
   dotnet publish -c Release
   ```

## Environment-Specific Notes

### Development
- Use local SQL Server or connect to dev database
- Email testing: Use Mailtrap or similar testing service
- Debug logging enabled

### Production
- Connection strings in environment variables
- Email: Production SMTP server
- HTTPS enforced
- Logging to Application Insights or similar

## Code Style Preferences

- Use `var` for local variables when type is obvious
- Async all the way (no sync-over-async)
- Prefer `IDbContextFactory` over direct DbContext injection
- Use `await using` for DbContext disposal
- Validate input on both client and server
- Keep Razor components focused (separate logic into services)
- Use nullable reference types

## Key Files to Review

When working on this project, familiarize yourself with:

1. `Program.cs` - Service registration and middleware pipeline
2. `AppDbContext.cs` - Main database context and entity configuration
3. `AionAccountsDbContext.cs` - Legacy Aion database (read-only)
4. `ApplicationUser.cs` - Custom Identity user model
5. `AccountService.cs` - Integration layer between Identity and Aion accounts
6. `EmailService.cs` - Email sending implementation
7. `RoleSeeder.cs` - Admin initialization

## Next Steps / Future Enhancements

- [ ] Implement user profile management
- [ ] Add 2FA (two-factor authentication)
- [ ] Implement admin panel for content management
- [ ] Add forum/community features
- [ ] Implement character management (link to Aion database)
- [ ] Add news article creation/editing (admin only)
- [ ] Implement wiki editing system
- [ ] Add donation/payment integration
- [ ] Implement server status monitoring
- [ ] Add Discord bot integration

## Support & Troubleshooting

If you encounter issues:
1. Check the `/Documentation` folder for guides
2. Review error logs (default: `bin/Debug/net10.0/logs/`)
3. Verify database connection strings
4. Confirm email service configuration
5. Check EF Core migrations are up to date

---

**Last Updated**: Based on development session implementing ASP.NET Core Identity with email verification, password reset, and PIN recovery features.
