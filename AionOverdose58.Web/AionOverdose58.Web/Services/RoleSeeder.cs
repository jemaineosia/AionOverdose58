using AionOverdose58.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AionOverdose58.Web.Services;

/// <summary>
/// Service to seed default roles and admin user.
/// </summary>
public static class RoleSeeder
{
    /// <summary>
    /// Seeds default roles and creates admin user if not exists.
    /// </summary>
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            // Define roles
            string[] roles = { "Admin", "Moderator", "Player", "VIP" };

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Created role: {Role}", role);
                }
            }

            // Create default admin user if it doesn't exist
            const string adminUsername = "admin";
            const string adminEmail = "admin@aionoverdose58.com";
            const string adminPassword = "Admin123!"; // Change this in production!

            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PinCode = "1234",
                    RegisteredDate = DateTime.UtcNow,
                    IsActive = true,
                    IsEmailVerified = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    logger.LogInformation("Admin user created successfully");
                }
                else
                {
                    logger.LogError("Failed to create admin user: {Errors}", 
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding roles and admin user");
        }
    }
}
