using AionOverdose58.Shared.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AionOverdose58.Web.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmationLink);
    Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetLink);
    Task SendWelcomeEmailAsync(string toEmail, string userName);
    Task SendPasswordChangedNotificationAsync(string toEmail, string userName, string ipAddress);
}

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, 
                _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
            
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
            throw;
        }
    }

    public async Task SendConfirmationEmailAsync(string toEmail, string userName, string confirmationLink)
    {
        var subject = "Confirm Your Email - Aion Overdose 58";
        var htmlMessage = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #0a0e1a;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #1a1f2e;
            border: 1px solid #c9a84c;
            border-radius: 8px;
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #c9a84c 0%, #8b1a1a 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            color: #ffffff;
            font-size: 28px;
            text-transform: uppercase;
            letter-spacing: 2px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #c9a84c;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #cccccc;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            margin: 20px 0;
            background: linear-gradient(135deg, #c9a84c 0%, #b8941f 100%);
            color: #0a0e1a;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .footer {{
            background-color: #0f1419;
            padding: 20px;
            text-align: center;
            color: #666666;
            font-size: 12px;
        }}
        .divider {{
            height: 1px;
            background: linear-gradient(90deg, transparent, #c9a84c, transparent);
            margin: 20px 0;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>☠ Aion Overdose 58 ☠</h1>
        </div>
        <div class=""content"">
            <h2>Welcome, {userName}!</h2>
            <p>Thank you for registering with Aion Overdose 58. To complete your registration and activate your account, please confirm your email address by clicking the button below:</p>
            
            <div style=""text-align: center;"">
                <a href=""{confirmationLink}"" class=""button"">Confirm Email Address</a>
            </div>
            
            <div class=""divider""></div>
            
            <p>If the button doesn't work, copy and paste this link into your browser:</p>
            <p style=""word-break: break-all; color: #c9a84c;"">{confirmationLink}</p>
            
            <p><strong>This link will expire in 24 hours.</strong></p>
            
            <div class=""divider""></div>
            
            <p>If you didn't create an account with Aion Overdose 58, please ignore this email.</p>
            
            <p style=""margin-top: 30px;"">
                <strong>Ready to begin your journey?</strong><br>
                Once confirmed, you'll be able to:
            </p>
            <ul style=""color: #cccccc;"">
                <li>Access your account dashboard</li>
                <li>Download the game client</li>
                <li>Join our community</li>
                <li>Start your adventure in Atreia!</li>
            </ul>
        </div>
        <div class=""footer"">
            <p>Aion Overdose 58 - Middle Rate • Official Build • Aion 4.9</p>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(toEmail, subject, htmlMessage);
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetLink)
    {
        var subject = "Reset Your Password - Aion Overdose 58";
        var htmlMessage = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #0a0e1a;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #1a1f2e;
            border: 1px solid #c9a84c;
            border-radius: 8px;
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #8b1a1a 0%, #c9a84c 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            color: #ffffff;
            font-size: 28px;
            text-transform: uppercase;
            letter-spacing: 2px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #c9a84c;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #cccccc;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            margin: 20px 0;
            background: linear-gradient(135deg, #8b1a1a 0%, #b8321f 100%);
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .footer {{
            background-color: #0f1419;
            padding: 20px;
            text-align: center;
            color: #666666;
            font-size: 12px;
        }}
        .warning {{
            background-color: #8b1a1a33;
            border-left: 4px solid #8b1a1a;
            padding: 15px;
            margin: 20px 0;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>☠ Aion Overdose 58 ☠</h1>
        </div>
        <div class=""content"">
            <h2>Password Reset Request</h2>
            <p>Hello {userName},</p>
            <p>We received a request to reset your password. Click the button below to create a new password:</p>
            
            <div style=""text-align: center;"">
                <a href=""{resetLink}"" class=""button"">Reset Password</a>
            </div>
            
            <p>Or copy and paste this link into your browser:</p>
            <p style=""word-break: break-all; color: #c9a84c;"">{resetLink}</p>
            
            <div class=""warning"">
                <strong>⚠️ Security Notice:</strong><br>
                This link will expire in 1 hour. If you didn't request a password reset, please ignore this email and your password will remain unchanged.
            </div>
            
            <p>For your security, we recommend:</p>
            <ul style=""color: #cccccc;"">
                <li>Using a strong, unique password</li>
                <li>Not sharing your password with anyone</li>
                <li>Enabling two-factor authentication (if available)</li>
            </ul>
        </div>
        <div class=""footer"">
            <p>Aion Overdose 58 - Middle Rate • Official Build • Aion 4.9</p>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(toEmail, subject, htmlMessage);
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string userName)
    {
        var subject = "Welcome to Aion Overdose 58!";
        var htmlMessage = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #0a0e1a;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #1a1f2e;
            border: 1px solid #c9a84c;
            border-radius: 8px;
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #c9a84c 0%, #8b1a1a 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            color: #ffffff;
            font-size: 28px;
            text-transform: uppercase;
            letter-spacing: 2px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #c9a84c;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #cccccc;
        }}
        .feature-box {{
            background-color: #0f1419;
            border: 1px solid #c9a84c33;
            padding: 15px;
            margin: 10px 0;
            border-radius: 5px;
        }}
        .feature-box strong {{
            color: #c9a84c;
        }}
        .footer {{
            background-color: #0f1419;
            padding: 20px;
            text-align: center;
            color: #666666;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>☠ Welcome to Aion Overdose 58! ☠</h1>
        </div>
        <div class=""content"">
            <h2>Your Account is Ready, {userName}!</h2>
            <p>Congratulations! Your email has been confirmed and your account is now fully activated.</p>
            
            <p><strong>What's Next?</strong></p>
            
            <div class=""feature-box"">
                <strong>📥 Download the Client</strong><br>
                Get the full Aion 4.9 client to start playing. Multiple mirrors available for fast downloads.
            </div>
            
            <div class=""feature-box"">
                <strong>⚔️ Server Features</strong><br>
                • EXP Rates: x5 (Monsters & Quests)<br>
                • Drop Rate: x2.5<br>
                • Craft/Gather: x3<br>
                • No Pay-to-Win
            </div>
            
            <div class=""feature-box"">
                <strong>👥 Join Our Community</strong><br>
                Connect with other players on Discord for updates, events, and support.
            </div>
            
            <div class=""feature-box"">
                <strong>📰 Stay Updated</strong><br>
                Check our news page regularly for server updates, events, and patch notes.
            </div>
            
            <p style=""margin-top: 30px;"">
                <strong>Need Help?</strong><br>
                Visit our website or join our Discord server if you have any questions. Our community is here to help!
            </p>
            
            <p style=""margin-top: 30px; text-align: center; font-size: 18px; color: #c9a84c;"">
                <strong>See you in Atreia, Daeva!</strong>
            </p>
        </div>
        <div class=""footer"">
            <p>Aion Overdose 58 - Middle Rate • Official Build • Aion 4.9</p>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(toEmail, subject, htmlMessage);
    }

    public async Task SendPasswordChangedNotificationAsync(string toEmail, string userName, string ipAddress)
    {
        var subject = "Password Changed - Aion Overdose 58";
        var htmlMessage = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #0a0e1a;
            color: #ffffff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #1a1f2e;
            border: 1px solid #c9a84c;
            border-radius: 8px;
            overflow: hidden;
        }}
        .header {{
            background: linear-gradient(135deg, #8b1a1a 0%, #c9a84c 100%);
            padding: 30px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            color: #ffffff;
            font-size: 28px;
            text-transform: uppercase;
            letter-spacing: 2px;
        }}
        .content {{
            padding: 40px 30px;
        }}
        .content h2 {{
            color: #c9a84c;
            margin-top: 0;
        }}
        .content p {{
            line-height: 1.6;
            color: #cccccc;
        }}
        .info-box {{
            background-color: #0f1419;
            border-left: 4px solid #c9a84c;
            padding: 15px;
            margin: 20px 0;
        }}
        .warning {{
            background-color: #8b1a1a33;
            border-left: 4px solid #8b1a1a;
            padding: 15px;
            margin: 20px 0;
        }}
        .button {{
            display: inline-block;
            padding: 15px 40px;
            margin: 20px 0;
            background: linear-gradient(135deg, #8b1a1a 0%, #b8321f 100%);
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 1px;
        }}
        .footer {{
            background-color: #0f1419;
            padding: 20px;
            text-align: center;
            color: #666666;
            font-size: 12px;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>☠ Aion Overdose 58 ☠</h1>
        </div>
        <div class=""content"">
            <h2>🔐 Password Changed</h2>
            <p>Hello {userName},</p>
            <p>This is a notification that your password was recently changed.</p>

            <div class=""info-box"">
                <strong>Change Details:</strong><br>
                • Date/Time: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC<br>
                • IP Address: {ipAddress}<br>
                • Action: Password Reset
            </div>

            <p>If you made this change, you can safely ignore this email. Your account is secure.</p>

            <div class=""warning"">
                <strong>⚠️ Didn't Change Your Password?</strong><br><br>
                If you did NOT request this password change, your account may be compromised. Please take action immediately:
                <ul>
                    <li>Reset your password again using a secure device</li>
                    <li>Enable two-factor authentication (if available)</li>
                    <li>Contact our support team</li>
                    <li>Review recent account activity</li>
                </ul>
            </div>

            <p><strong>Security Recommendations:</strong></p>
            <ul style=""color: #cccccc;"">
                <li>Use a unique password for this account</li>
                <li>Never share your password with anyone</li>
                <li>Be cautious of phishing attempts</li>
                <li>Keep your email account secure</li>
            </ul>

            <p>If you have any concerns about your account security, please contact our support team immediately.</p>
        </div>
        <div class=""footer"">
            <p>Aion Overdose 58 - Middle Rate • Official Build • Aion 4.9</p>
            <p>This is an automated security notification. Please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";

        await SendEmailAsync(toEmail, subject, htmlMessage);
    }
}
