using AionOverdose58.Data;
using AionOverdose58.Shared.Models;
using AionOverdose58.Shared.Models.Aion;
using AionOverdose58.Shared.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
}

public class AccountService : IAccountService
{
    private readonly IDbContextFactory<AppDbContext> _webDbFactory;
    private readonly IDbContextFactory<AionAccountsDbContext> _aionDbFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        IDbContextFactory<AppDbContext> webDbFactory,
        IDbContextFactory<AionAccountsDbContext> aionDbFactory,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ILogger<AccountService> logger)
    {
        _webDbFactory = webDbFactory;
        _aionDbFactory = aionDbFactory;
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
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
                EmailConfirmed = false,
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

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var confirmationLink = $"{baseUrl}/account/confirm-email?userId={user.Id}&code={encodedToken}";

            // Send confirmation email
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
            }
            else
            {
                _logger.LogWarning("Stored procedure returned {ReturnValue} for user: {Username}", aionAccountUid, username);
            }

            _logger.LogInformation("Account created successfully for user: {Username} with AionUID: {AionUid}", username, aionAccountUid);
            return (true, "Registration successful! Please check your email to confirm your account.");
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
}
