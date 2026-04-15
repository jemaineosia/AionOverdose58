# Quick Reference - Password Reset & Email Resend

## 🚀 Pages Overview

| Page | URL | Purpose |
|------|-----|---------|
| **Resend Confirmation** | `/account/resend-confirmation` | Resend email verification link |
| **Forgot Password** | `/account/forgot-password` | Request password reset link |
| **Reset Password** | `/account/reset-password` | Actually reset the password |

---

## 📧 Email Flows

### **Resend Confirmation**
```
Input: Email address
   ↓
Validates: Email exists & not confirmed
   ↓
Sends: Confirmation email (24hr token)
   ↓
User: Clicks link
   ↓
Result: Email confirmed + Welcome email
```

### **Password Reset**
```
Input: Email address
   ↓
Validates: Email exists & confirmed
   ↓
Sends: Reset email (24hr token)
   ↓
User: Clicks link → Reset password page
   ↓
Input: New password (6-16 chars)
   ↓
Result: Password updated (Web + Game DB)
```

---

## 🔒 Security Features

| Feature | Setting |
|---------|---------|
| **Confirmation Token** | 24 hours expiration |
| **Reset Token** | 24 hours expiration |
| **User Enumeration** | Protected (generic messages) |
| **Password Requirements** | 6-16 chars, letter + number |
| **Database Updates** | Both Web & Aion databases |
| **Token Type** | One-time use, cryptographically secure |

---

## 🧪 Quick Tests

### **Test Resend Confirmation:**
```bash
1. Go to: /account/resend-confirmation
2. Enter: registered email
3. Check: email inbox
4. Click: confirmation link
5. Verify: welcome email received
```

### **Test Password Reset:**
```bash
1. Go to: /account/forgot-password
2. Enter: confirmed email
3. Check: email inbox
4. Click: reset link
5. Enter: new password (twice)
6. Login: with new password
7. Game: test game login
```

---

## 📊 Database Updates on Password Reset

### **Web Database (AionGameCP):**
```sql
UPDATE AspNetUsers 
SET PasswordHash = '<new-bcrypt-hash>'
WHERE Email = 'user@email.com'
```

### **Game Database (AionAccounts):**
```sql
UPDATE user_auth
SET 
    password = <binary-encrypted>,      -- 16 bytes
    passwd = '<sha1-base64>',           -- Web password
    web_password = '0x<hex-encrypted>'  -- Hex format
WHERE account = 'username'
```

---

## 🎨 UI States

### **Processing:**
- Spinner animation
- "Sending..." or "Processing..." text
- Button disabled

### **Success:**
- ✅ Green checkmark
- Success message
- Next action button (Login, etc.)

### **Error:**
- ❌ Red X icon
- Helpful error message
- Retry or alternative options

---

## 💡 Common Error Messages

| Message | Meaning | Action |
|---------|---------|--------|
| "Email sent" | Generic success (security) | Check inbox |
| "Email already confirmed" | Account ready | Go to login |
| "Invalid link" | Token expired/wrong | Request new link |
| "Passwords don't match" | Mismatch | Re-enter passwords |
| "Link expired" | 24 hours passed | Request new link |

---

## 🔧 Configuration Locations

### **Program.cs:**
```csharp
// Token lifespan
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24);
});

// Email service
builder.Services.AddScoped<IEmailService, EmailService>();

// Account service
builder.Services.AddScoped<IAccountService, AccountService>();
```

### **appsettings.json:**
```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "Username": "your-email@gmail.com",
  "Password": "your-app-password",
  "EnableSsl": true
}
```

---

## 🎯 Service Methods

### **AccountService:**

```csharp
// Resend confirmation
var (success, message) = await AccountService.ResendConfirmationEmailAsync(
    email, 
    baseUrl
);

// Forgot password
var (success, message) = await AccountService.ForgotPasswordAsync(
    email, 
    baseUrl
);

// Reset password
var (success, message) = await AccountService.ResetPasswordAsync(
    email, 
    code, 
    newPassword
);
```

---

## 📱 User Journey Map

### **Can't Login → Forgot Password:**
```
Login Page
   ↓
Click "Forgot Password"
   ↓
Enter Email → Submit
   ↓
Check Email
   ↓
Click Reset Link
   ↓
Enter New Password
   ↓
Success → Login
```

### **Didn't Get Confirmation → Resend:**
```
Can't Login (Email not confirmed)
   ↓
Click "Resend Confirmation"
   ↓
Enter Email → Submit
   ↓
Check Email (+ Spam!)
   ↓
Click Confirmation Link
   ↓
Email Confirmed → Welcome Email
   ↓
Login
```

---

## 🚨 Troubleshooting

### **"Email not received"**
1. Check spam/junk folder
2. Wait 5 minutes
3. Verify email address correct
4. Check SMTP logs
5. Resend email

### **"Invalid token" error**
1. Check link is complete
2. Verify within 24 hours
3. Request new reset link
4. Clear browser cache
5. Try different browser

### **"Password not updating in game"**
1. Check Aion database connection
2. Verify user_auth record exists
3. Check application logs
4. Test SMTP connection
5. Manually verify in database

---

## 📋 Feature Checklist

### **Resend Confirmation:**
- [x] Page created
- [x] Email validation
- [x] Token generation
- [x] Email sending
- [x] Success/error states
- [x] Security measures
- [x] Help section
- [x] Mobile responsive

### **Password Reset:**
- [x] Forgot password page
- [x] Reset password page
- [x] Token validation
- [x] Password strength rules
- [x] Email sending
- [x] Web DB update
- [x] Game DB update
- [x] Success confirmation

---

## 🎓 Quick Tips

1. **Always check spam folder** when testing
2. **Tokens expire after 24 hours** - test quickly
3. **Generic messages protect security** - "email sent" doesn't reveal user existence
4. **Password must be 6-16 characters** with letter + number
5. **Both databases update** on password reset
6. **Email must be confirmed** before password reset
7. **Use app passwords** for Gmail SMTP
8. **Test mobile view** - all pages responsive

---

## 🔗 Related Links

- **Full Guide:** [Password-Reset-Guide.md](Password-Reset-Guide.md)
- **Email Setup:** [Email-Quick-Setup.md](Email-Quick-Setup.md)
- **Identity Guide:** [Identity-Integration-Guide.md](Identity-Integration-Guide.md)

---

## ⚡ Quick Actions

### **Enable in Production:**
1. Set correct SMTP settings in `appsettings.json`
2. Test all email flows
3. Add rate limiting (recommended)
4. Monitor logs for errors
5. Test with real email accounts

### **Security Hardening:**
1. Add CAPTCHA to forgot password
2. Rate limit to 5 requests per hour per IP
3. Email notification on password change
4. Monitor for suspicious activity
5. Log all password reset attempts

---

**✅ All Features Working!**

- Resend confirmation email ✓
- Forgot password ✓
- Reset password ✓
- Email notifications ✓
- Database synchronization ✓
- Security best practices ✓

**Your account recovery system is complete!** 🎉🔐📧
