using Appointments.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Appointments.Infrastructure.Persistence.Seed;

public class ApplicationDbContextSeed
{
    public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "Administrator" });
            await roleManager.CreateAsync(new ApplicationRole { Name = "Patient" });
        }
    }

    public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        if (userManager.Users.All(u => u.UserName != "admin"))
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@localhost.com",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Administrator");
            
        }
    }
    ///create other user client
    
    public static async Task SeedClientUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        if (userManager.Users.All(u => u.UserName != "client"))
        {
            var clientUser = new ApplicationUser
            {
                UserName = "client",
                Email = "client@localhost.com",
                FirstName = "System",
                LastName = "client",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(clientUser, "Admin123!");
            await userManager.AddToRoleAsync(clientUser, "Administrator");
            
        }
    }
}
