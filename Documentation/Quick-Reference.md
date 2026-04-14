# Quick Reference - Identity System

## 🚀 Getting Started

### Test the System

1. **Run the application:**
   ```bash
   dotnet run --project AionOverdose58.Web\AionOverdose58.Web\AionOverdose58.Web.csproj
   ```

2. **Default Admin Login:**
   - URL: `https://localhost:7XXX/account/login`
   - Username: `admin`
   - Password: `Admin123!`

3. **Register New User:**
   - URL: `https://localhost:7XXX/account/register`
   - Fill in all fields
   - Complete captcha
   - Submit

---

## 💾 Database Connection Strings

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=158.69.54.217;Database=AionGameCP;User Id=sa;Password=4i0nOverD0s3!@;...",
    "AionAccountsConnection": "Server=158.69.54.217;Database=AionAccounts;User Id=sa;Password=4i0nOverD0s3!@;..."
  }
}
```

---

## 📊 Quick SQL Queries

### Check Identity Users
```sql
USE AionGameCP
GO

-- All users
SELECT Id, UserName, Email, EmailConfirmed, RegisteredDate, AionAccountUid, IsActive
FROM AspNetUsers

-- Users with roles
SELECT u.UserName, u.Email, r.Name AS Role
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
```

### Check Aion Accounts
```sql
USE AionAccounts
GO

-- Recently created accounts
SELECT TOP 10
    ui.account,
    s.email,
    s.mobile AS PIN,
    ui.create_date,
    ua.uid
FROM user_info ui
INNER JOIN ssn s ON ui.ssn = s.ssn
INNER JOIN user_account ua ON ui.account = ua.account
ORDER BY ui.create_date DESC
```

### Verify Synchronization
```sql
-- Check if Identity user has Aion account
USE AionGameCP
GO

SELECT 
    u.UserName,
    u.Email,
    u.AionAccountUid,
    CASE 
        WHEN u.AionAccountUid IS NOT NULL THEN 'Linked'
        ELSE 'Not Linked'
    END AS Status
FROM AspNetUsers u
```

---

## 🔑 Common Tasks

### Add User to Role
```csharp
await userManager.AddToRoleAsync(user, "VIP");
```

### Remove User from Role
```csharp
await userManager.RemoveFromRoleAsync(user, "VIP");
```

### Check User Role
```csharp
var isAdmin = await userManager.IsInRoleAsync(user, "Admin");
```

### Lock User Account
```csharp
await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddHours(24));
```

### Unlock User Account
```csharp
await userManager.SetLockoutEndDateAsync(user, null);
```

---

## 🎨 UI Components

### Show User Info
```razor
<AuthorizeView>
    <Authorized>
        <p>Welcome, @context.User.Identity!.Name</p>
    </Authorized>
    <NotAuthorized>
        <a href="/account/login">Login</a>
    </NotAuthorized>
</AuthorizeView>
```

### Admin Only Section
```razor
<AuthorizeView Roles="Admin">
    <Authorized>
        <a href="/admin">Admin Panel</a>
    </Authorized>
</AuthorizeView>
```

### Logout Button
```razor
@inject SignInManager<ApplicationUser> SignInManager
@inject NavigationManager Navigation

<button @onclick="Logout" class="btn btn-danger">
    Logout
</button>

@code {
    private async Task Logout()
    {
        await SignInManager.SignOutAsync();
        Navigation.NavigateTo("/", forceLoad: true);
    }
}
```

---

## 🔍 Troubleshooting

### User Can't Login

**Check:**
1. Is account locked? 
   ```sql
   SELECT LockoutEnd FROM AspNetUsers WHERE UserName = 'username'
   ```
2. Is password correct? (reset if needed)
3. Check logs for errors

### Account Not Created in Aion Database

**Check:**
1. Stored procedure execution
2. SQL Server logs
3. AionAccountUid in AspNetUsers (should be > 0)

### Duplicate Email/Username Error

**Verify uniqueness:**
```sql
-- Check Identity
SELECT COUNT(*) FROM AspNetUsers WHERE Email = 'test@email.com'

-- Check Aion
SELECT COUNT(*) FROM ssn WHERE email = 'test@email.com'
```

---

## 📈 Performance Tips

1. **Use caching for roles:**
   ```csharp
   services.AddMemoryCache();
   ```

2. **Index important columns:**
   ```sql
   CREATE INDEX IX_AspNetUsers_AionAccountUid ON AspNetUsers(AionAccountUid)
   ```

3. **Connection pooling:**
   - Already enabled in connection strings
   - `MultipleActiveResultSets=true`

---

## 🔒 Security Checklist

- [ ] Changed default admin password
- [ ] HTTPS enabled in production
- [ ] Strong password policy configured
- [ ] Account lockout enabled
- [ ] Captcha on registration
- [ ] SQL injection prevention (parameterized queries ✅)
- [ ] XSS protection (HttpOnly cookies ✅)
- [ ] CSRF protection (Antiforgery tokens ✅)
- [ ] Regular security audits
- [ ] Database backups automated

---

## 📞 Support

**Common Issues:**
- Build errors → Check NuGet packages installed
- Migration errors → Check connection string
- Login not working → Verify roles seeded
- Aion account not created → Check stored procedure

**Logs Location:**
- Application logs: Console output
- SQL Server logs: SQL Server Management Studio
- Identity logs: Configured in `appsettings.json`

---

## 🎯 Files Modified/Created

### New Files:
- ✅ `ApplicationUser.cs` - Extended identity user
- ✅ `RoleSeeder.cs` - Seeds default roles
- ✅ `Login.razor` - Login page with Identity
- ✅ `Register.razor` - Updated with Identity
- ✅ `Identity-Integration-Guide.md` - Full documentation

### Modified Files:
- ✅ `AppDbContext.cs` - Now extends IdentityDbContext
- ✅ `AccountService.cs` - Uses UserManager
- ✅ `Program.cs` - Identity configuration added

### Database Changes:
- ✅ `AionGameCP` - Identity tables created
- ✅ Roles seeded (Admin, Moderator, Player, VIP)
- ✅ Default admin user created

---

**System Status: ✅ READY FOR USE**

All components integrated successfully. You can now:
- Register new users
- Login with username or email
- Assign roles
- Manage users through Identity
- Game accounts sync automatically

Happy coding! 🎮✨
