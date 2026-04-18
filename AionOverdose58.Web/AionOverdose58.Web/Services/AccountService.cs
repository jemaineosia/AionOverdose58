using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using AionOverdose58.Shared.Models.Aion;
using AionOverdose58.Shared.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Text;
using System.Web;

namespace AionOverdose58.Web.Services;

public interface IAccountService
{
    Task<(bool Success, string Message)> RegisterAccountAsync(string username, string email, string password, string pinCode, string baseUrl);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<(bool Success, string Message)> ConfirmEmailAsync(string userId, string code);
    Task<(bool Success, string Message)> ResendConfirmationEmailAsync(string email, string baseUrl);
    Task<(bool Success, string Message)> ForgotPasswordAsync(string email, string baseUrl);
    Task<(bool Success, string Message)> ResetPasswordAsync(string email, string code, string newPassword, string ipAddress);
    Task<(bool Success, string Message)> RecoverAccountWithPinAsync(string email, string pinCode, string baseUrl);
    Task<(bool Success, string Message)> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<(bool Success, string Message)> ChangePinAsync(string userId, string currentPin, string newPin);
    Task<(bool Success, string Message)> ChangeEmailAsync(string userId, string newEmail, string baseUrl);
}

public class AccountService : IAccountService
{
    private readonly IDbContextFactory<AppDbContext> _webDbFactory;
    private readonly IDbContextFactory<AionAccountsDbContext> _aionDbFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountService> _logger;
    private readonly AccountSettings _accountSettings;

    public AccountService(
        IDbContextFactory<AppDbContext> webDbFactory,
        IDbContextFactory<AionAccountsDbContext> aionDbFactory,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ILogger<AccountService> logger,
        IOptions<AccountSettings> accountSettings)
    {
        _webDbFactory = webDbFactory;
        _aionDbFactory = aionDbFactory;
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
        _accountSettings = accountSettings.Value;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        try
        {
            // Check in Identity
            var identityUser = await _userManager.FindByNameAsync(username);
            if (identityUser != null) return true;

            // Check in Aion database
            await using var aionDb = await _aionDbFactory.CreateDbContextAsync();
            var existsInAion = await aionDb.UserInfos.AnyAsync(x => x.Account == username);

            return existsInAion;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking username existence: {Username}", username);
            return true; // Return true to prevent registration on error
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        try
        {
            // Check in Identity
            var identityUser = await _userManager.FindByEmailAsync(email);
            if (identityUser != null) return true;

            // Check in Aion database
            await using var aionDb = await _aionDbFactory.CreateDbContextAsync();
            var existsInAion = await aionDb.Ssns.AnyAsync(x => x.Email == email);

            return existsInAion;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking email existence: {Email}", email);
            return true; // Return true to prevent registration on error
        }
    }

    public async Task<(bool Success, string Message)> RegisterAccountAsync(
        string username, 
        string email, 
        string password, 
        string pinCode,
        string baseUrl)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || 
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(pinCode))
            {
                return (false, "All fields are required.");
            }

            if (username.Length < 6 || username.Length > 20)
            {
                return (false, "Username must be between 6 and 20 characters.");
            }

            if (password.Length < 6 || password.Length > 16)
            {
                return (false, "Password must be between 6 and 16 characters.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(pinCode, @"^\d{4,6}$"))
            {
                return (false, "PIN code must be 4-6 digits.");
            }

            // Check for duplicate username and email
            if (await UsernameExistsAsync(username))
            {
                return (false, "Username already exists.");
            }

            if (await EmailExistsAsync(email))
            {
                return (false, "Email already exists.");
            }

            // Create Identity user
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                PinCode = pinCode,
                RegisteredDate = DateTime.UtcNow,
                EmailConfirmed = !_accountSettings.RequireEmailConfirmation,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create Identity user: {Errors}", errors);
                return (false, $"Failed to create account: {errors}");
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, "Player");

            // Send confirmation email if required
            if (_accountSettings.RequireEmailConfirmation)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = HttpUtility.UrlEncode(token);
                var confirmationLink = $"{baseUrl}/account/confirm-email?userId={user.Id}&code={encodedToken}";

                try
                {
                    await _emailService.SendConfirmationEmailAsync(email, username, confirmationLink);
                    _logger.LogInformation("Confirmation email sent to {Email}", email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send confirmation email to {Email}", email);
                    // Don't fail registration if email fails
                }
            }

            // Create account in Aion database using stored procedure
            await using var aionDb = await _aionDbFactory.CreateDbContextAsync();

            var returnValueParameter = new SqlParameter("@ReturnVal", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var guid = Guid.NewGuid().ToString();

            await aionDb.Database.ExecuteSqlRawAsync(
                "EXEC @ReturnVal = od_CreateAccount @ggid, @account, @password, @email, @mobile, @question1, @question2, @answer1, @answer2, @passwd, @web_password",
                new SqlParameter("@ggid", guid),
                new SqlParameter("@account", username),
                new SqlParameter("@password", AionEncrypt.EncryptPasswordInByte(password)),
                new SqlParameter("@email", email),
                new SqlParameter("@mobile", pinCode),
                new SqlParameter("@question1", string.Empty),
                new SqlParameter("@question2", string.Empty),
                new SqlParameter("@answer1", new byte[1]),
                new SqlParameter("@answer2", new byte[1]),
                new SqlParameter("@passwd", AionEncrypt.EncryptWebPassword(password)),
                new SqlParameter("@web_password", "0x" + AionEncrypt.EncryptPassword(password).ToUpper()),
                returnValueParameter
            );

            var aionAccountUid = (int)returnValueParameter.Value;

            if (aionAccountUid > 0)
            {
                // Update Identity user with Aion account UID
                user.AionAccountUid = aionAccountUid;
                await _userManager.UpdateAsync(user);

                // Always sync the password to account_data in case the stored procedure
                // returned an existing account UID without updating its password
                await aionDb.Database.ExecuteSqlRawAsync(
                    @"UPDATE account_data SET 
                        password = @password,
                        passwd   = @passwd,
                        web_password = @web_password
                      WHERE id = @uid",
                    new SqlParameter("@password", AionEncrypt.EncryptPasswordInByte(password)),
                    new SqlParameter("@passwd", AionEncrypt.EncryptWebPassword(password)),
                    new SqlParameter("@web_password", "0x" + AionEncrypt.EncryptPassword(password).ToUpper()),
                    new SqlParameter("@uid", aionAccountUid)
                );
            }
            else
            {
                _logger.LogWarning("Stored procedure returned {ReturnValue} for user: {Username}", aionAccountUid, username);
            }

            _logger.LogInformation("Account created successfully for user: {Username} with AionUID: {AionUid}", username, aionAccountUid);

            var message = _accountSettings.RequireEmailConfirmation
                ? "Registration successful! Please check your email to confirm your account."
                : "Registration successful! You can now log in.";

            return (true, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering account for username: {Username}", username);
            return (false, "An error occurred during registration. Please try again.");
        }
    }

    public async Task<(bool Success, string Message)> ConfirmEmailAsync(string userId, string code)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "User not found.");
            }

            if (user.EmailConfirmed)
            {
                return (false, "Email already confirmed.");
            }

            var decodedToken = HttpUtility.UrlDecode(code);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            {
                // Update IsEmailVerified
                user.IsEmailVerified = true;
                await _userManager.UpdateAsync(user);

                // Send welcome email
                try
                {
                    await _emailService.SendWelcomeEmailAsync(user.Email!, user.UserName!);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send welcome email to {Email}", user.Email);
                }

                _logger.LogInformation("Email confirmed for user: {Username}", user.UserName);
                return (true, "Email confirmed successfully! You can now log in.");
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Email confirmation failed for user {Username}: {Errors}", user.UserName, errors);
            return (false, "Email confirmation failed. The link may be expired or invalid.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming email for user {UserId}", userId);
            return (false, "An error occurred during email confirmation.");
        }
    }

    public async Task<(bool Success, string Message)> ResendConfirmationEmailAsync(string email, string baseUrl)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal that user doesn't exist for security
                return (true, "If an account exists with this email, a confirmation link has been sent.");
            }

            if (user.EmailConfirmed)
            {
                return (false, "This email is already confirmed. You can log in now.");
            }

            // Generate new confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"{baseUrl}/account/confirm-email?userId={user.Id}&code={encodedToken}";

            // Send confirmation email
            await _emailService.SendConfirmationEmailAsync(email, user.UserName!, confirmationLink);

            _logger.LogInformation("Confirmation email resent to {Email}", email);
            return (true, "Confirmation email sent! Please check your inbox.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resending confirmation email to {Email}", email);
            return (false, "An error occurred. Please try again later.");
        }
    }

    public async Task<(bool Success, string Message)> ForgotPasswordAsync(string email, string baseUrl)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
            {
                // Don't reveal that user doesn't exist or email not confirmed for security
                return (true, "If an account exists with this email, a password reset link has been sent.");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetLink = $"{baseUrl}/account/reset-password?email={HttpUtility.UrlEncode(email)}&code={encodedToken}";

            // Send password reset email
            await _emailService.SendPasswordResetEmailAsync(email, user.UserName!, resetLink);

            _logger.LogInformation("Password reset email sent to {Email}", email);
            return (true, "Password reset link sent! Please check your email.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset email to {Email}", email);
            return (false, "An error occurred. Please try again later.");
        }
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(string email, string code, string newPassword, string ipAddress)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "Invalid password reset request.");
            }

            if (!user.EmailConfirmed)
            {
                return (false, "Please confirm your email before resetting your password.");
            }

            // Validate password
            if (newPassword.Length < 6 || newPassword.Length > 16)
            {
                return (false, "Password must be between 6 and 16 characters.");
            }

            var decodedToken = HttpUtility.UrlDecode(code);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (result.Succeeded)
            {
                // Update Aion database password as well
                try
                {
                    await using var aionDb = await _aionDbFactory.CreateDbContextAsync();

                    var userAuth = await aionDb.UserAuths.FirstOrDefaultAsync(x => x.Account == user.UserName);
                    if (userAuth != null)
                    {
                        userAuth.Password = AionEncrypt.EncryptPasswordInByte(newPassword);
                        userAuth.Passwd = AionEncrypt.EncryptWebPassword(newPassword);
                        userAuth.WebPassword = "0x" + AionEncrypt.EncryptPassword(newPassword).ToUpper();
                        await aionDb.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Aion password for user: {Username}", user.UserName);
                }

                // Send password changed notification email
                try
                {
                    await _emailService.SendPasswordChangedNotificationAsync(user.Email!, user.UserName!, ipAddress);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send password change notification to {Email}", user.Email);
                }

                _logger.LogInformation("Password reset successfully for user: {Username} from IP: {IpAddress}", user.UserName, ipAddress);
                return (true, "Password reset successfully! You can now log in with your new password.");
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Password reset failed for user {Username}: {Errors}", user.UserName, errors);
            return (false, "Password reset failed. The link may be expired or invalid.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting password for email {Email}", email);
            return (false, "An error occurred during password reset.");
        }
    }

    public async Task<(bool Success, string Message)> RecoverAccountWithPinAsync(string email, string pinCode, string baseUrl)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal that user doesn't exist
                return (true, "If your PIN is correct, a password reset link has been sent to your email.");
            }

            // Verify PIN code
            if (user.PinCode != pinCode)
            {
                _logger.LogWarning("Invalid PIN attempt for email: {Email}", email);
                return (true, "If your PIN is correct, a password reset link has been sent to your email.");
            }

            if (!user.EmailConfirmed)
            {
                return (false, "Please confirm your email first. Check your inbox for the confirmation link.");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetLink = $"{baseUrl}/account/reset-password?email={HttpUtility.UrlEncode(email)}&code={encodedToken}";

            // Send password reset email
            await _emailService.SendPasswordResetEmailAsync(email, user.UserName!, resetLink);

            _logger.LogInformation("PIN recovery successful for email: {Email}", email);
            return (true, "PIN verified! Password reset link sent to your email.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during PIN recovery for email {Email}", email);
            return (false, "An error occurred. Please try again later.");
        }
    }

    public async Task<(bool Success, string Message)> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return (false, "User not found.");

            if (newPassword.Length < 6 || newPassword.Length > 16)
                return (false, "New password must be between 6 and 16 characters.");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description ?? "Password change failed.";
                return (false, error);
            }

            // Sync new password to Aion game database
            if (user.AionAccountUid.HasValue)
            {
                try
                {
                    await using var aionDb = await _aionDbFactory.CreateDbContextAsync();
                    await aionDb.Database.ExecuteSqlRawAsync(
                        @"UPDATE account_data SET 
                            password = @password,
                            passwd   = @passwd,
                            web_password = @web_password
                          WHERE id = @uid",
                        new SqlParameter("@password", AionEncrypt.EncryptPasswordInByte(newPassword)),
                        new SqlParameter("@passwd",   AionEncrypt.EncryptWebPassword(newPassword)),
                        new SqlParameter("@web_password", "0x" + AionEncrypt.EncryptPassword(newPassword).ToUpper()),
                        new SqlParameter("@uid", user.AionAccountUid.Value));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to sync new password to Aion DB for user {Username}", user.UserName);
                    // Don't fail the whole operation — web password is already changed
                }
            }

            _logger.LogInformation("Password changed for user {Username}", user.UserName);
            return (true, "Password changed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password for user {UserId}", userId);
            return (false, "An error occurred. Please try again.");
        }
    }

    public async Task<(bool Success, string Message)> ChangePinAsync(string userId, string currentPin, string newPin)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return (false, "User not found.");

            if (user.PinCode != currentPin)
                return (false, "Current PIN is incorrect.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(newPin, @"^\d{4,6}$"))
                return (false, "New PIN must be 4-6 digits.");

            user.PinCode = newPin;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return (false, "Failed to update PIN.");

            _logger.LogInformation("PIN changed for user {Username}", user.UserName);
            return (true, "PIN changed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing PIN for user {UserId}", userId);
            return (false, "An error occurred. Please try again.");
        }
    }

    public async Task<(bool Success, string Message)> ChangeEmailAsync(string userId, string newEmail, string baseUrl)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return (false, "User not found.");

            if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
                return (false, "New email is the same as your current email.");

            var existing = await _userManager.FindByEmailAsync(newEmail);
            if (existing != null)
                return (false, "This email address is already in use.");

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmLink = $"{baseUrl}/account/confirm-email-change?userId={user.Id}&newEmail={HttpUtility.UrlEncode(newEmail)}&code={encodedToken}";

            try
            {
                await _emailService.SendEmailAsync(newEmail, "Confirm your new email — Aion Overdose 58",
                    $"<p>Hello {user.UserName},</p><p>Click the link below to confirm your new email address:</p><p><a href='{confirmLink}'>Confirm Email Change</a></p>");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email-change confirmation to {Email}", newEmail);
                return (false, "Could not send confirmation email. Please try again.");
            }

            _logger.LogInformation("Email change requested for user {Username} -> {NewEmail}", user.UserName, newEmail);
            return (true, $"A confirmation link has been sent to {newEmail}. Click it to complete the change.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting email change for user {UserId}", userId);
            return (false, "An error occurred. Please try again.");
        }
    }
}
