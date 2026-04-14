# Quick Setup - Email Verification

## ⚡ 5-Minute Setup Guide

### **Step 1: Configure Gmail (Testing)**

1. **Enable 2FA on Gmail:**
   - https://myaccount.google.com/security
   - Turn on 2-Step Verification

2. **Generate App Password:**
   - https://myaccount.google.com/apppasswords
   - Select "Mail" and "Other"
   - Copy the 16-character password

3. **Update `appsettings.json`:**
```json
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "SmtpPort": 587,
  "SenderName": "Aion Overdose 58",
  "SenderEmail": "noreply@aionoverdose58.com",
  "Username": "YOUR-GMAIL@gmail.com",
  "Password": "YOUR-16-CHAR-APP-PASSWORD",
  "EnableSsl": true
}
```

### **Step 2: Test**

1. Run the application
2. Register a new account
3. Check your email (and spam folder!)
4. Click confirmation link
5. Login successfully

---

## 🧪 Quick Test Email

Add this to any Razor page:

```razor
@inject IEmailService EmailService

<button @onclick="TestEmail">Send Test Email</button>

@code {
    private async Task TestEmail()
    {
        try
        {
            await EmailService.SendEmailAsync(
                "your-test@email.com",
                "Test Email - Aion Overdose 58",
                "<h1 style='color: #c9a84c;'>Test Successful!</h1><p>Email configuration is working!</p>"
            );
            Console.WriteLine("✅ Email sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Email failed: {ex.Message}");
        }
    }
}
```

---

## 📧 Email Service URLs

### **Gmail**
- **SMTP:** smtp.gmail.com
- **Port:** 587 (TLS) or 465 (SSL)
- **App Passwords:** https://myaccount.google.com/apppasswords

### **SendGrid (Production Recommended)**
- **Website:** https://sendgrid.com
- **SMTP:** smtp.sendgrid.net
- **Port:** 587
- **Username:** apikey
- **Password:** (Your SendGrid API Key)
- **Free Tier:** 100 emails/day

### **Mailgun**
- **Website:** https://mailgun.com
- **SMTP:** smtp.mailgun.org
- **Port:** 587
- **Free Tier:** 5,000 emails/month

### **SMTP2GO**
- **Website:** https://www.smtp2go.com
- **SMTP:** mail.smtp2go.com
- **Port:** 2525, 8025, or 587
- **Free Tier:** 1,000 emails/month

---

## 🔍 Troubleshooting

### **Issue: Emails not sending**
```bash
# Check logs
dotnet run
# Look for errors in console

# Common fixes:
1. Verify SMTP settings
2. Check username/password
3. Enable "Less secure apps" (Gmail)
4. Use app password instead of regular password
5. Check firewall settings
```

### **Issue: Emails go to spam**
```
Solutions:
1. Add SPF record to your domain
2. Use a real domain email (not @gmail.com as sender)
3. Use SendGrid or Mailgun
4. Avoid spam trigger words
```

### **Issue: Confirmation link doesn't work**
```
Check:
1. Link is complete (not truncated)
2. Link hasn't expired (24 hours)
3. User hasn't already confirmed
4. Application is running and accessible
```

---

## 🎨 Email Templates Location

All email templates are in:
```
AionOverdose58.Web\AionOverdose58.Web\Services\EmailService.cs
```

**Methods:**
- `SendConfirmationEmailAsync()` - Registration confirmation
- `SendWelcomeEmailAsync()` - After email confirmed
- `SendPasswordResetEmailAsync()` - Password reset (future)

---

## 🔧 Configuration Files

### **appsettings.json**
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

### **Program.cs**
```csharp
// Email Settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

// Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Token lifespan (24 hours)
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(24);
});
```

---

## 🎯 Key Features

✅ **Email Confirmation Required**
- Users must confirm email before logging in
- Secure token-based verification
- 24-hour link expiration

✅ **Professional Email Templates**
- Branded HTML emails
- Dark theme matching your site
- Mobile-responsive design

✅ **Welcome Email**
- Sent automatically after confirmation
- Server features overview
- Getting started guide

✅ **Security**
- Prevents fake accounts
- Verifies email ownership
- Foundation for password reset

---

## 📱 Email Preview

### **Confirmation Email:**
```
☠ Aion Overdose 58 ☠

Welcome, USERNAME!

Thank you for registering with Aion Overdose 58...

[Confirm Email Address] <-- Big gold button

This link will expire in 24 hours.
```

### **Welcome Email:**
```
☠ Welcome to Aion Overdose 58! ☠

Your Account is Ready, USERNAME!

What's Next?
• Download the Client
• Server Features (EXP x5, Drop x2.5...)
• Join Our Community
• Stay Updated

See you in Atreia, Daeva!
```

---

## 🚀 Production Checklist

Before going live:

- [ ] Switch to SendGrid/Mailgun
- [ ] Use your own domain email
- [ ] Set up SPF/DKIM/DMARC
- [ ] Test email deliverability
- [ ] Monitor bounce rates
- [ ] Rate limit email sending
- [ ] Handle unsubscribes
- [ ] Privacy policy updated
- [ ] GDPR compliance

---

## 📊 Quick Stats

**Default Configuration:**
- Token Expiration: 24 hours
- Max Failed Logins: 5 attempts
- Lockout Duration: 15 minutes
- Email Required: Yes
- Email Confirmation Required: Yes

**Email Limits (Gmail):**
- Per day: 500 emails
- Per hour: ~100 emails
- Recommendation: Use SendGrid for production

---

## 💡 Pro Tips

1. **Test with Multiple Email Providers:**
   - Gmail, Outlook, Yahoo
   - Check spam folders
   - Verify HTML rendering

2. **Monitor Email Logs:**
   - Track sent emails
   - Monitor delivery rates
   - Watch for errors

3. **Provide Alternative:**
   - "Resend confirmation email" link
   - Contact support option
   - Manual verification (admin panel)

4. **User Experience:**
   - Clear instructions in email
   - Show success messages
   - Redirect after confirmation

---

## 🎓 Resources

- **Full Guide:** `Documentation\Email-Verification-Guide.md`
- **Identity Guide:** `Documentation\Identity-Integration-Guide.md`
- **Quick Reference:** `Documentation\Quick-Reference.md`

---

**🎉 Email Verification is Ready!**

Configure your SMTP settings in `appsettings.json` and start testing! 📧✨
