using System.Security.Claims;

using IdentityModel;

using IdentityService.Data;
using IdentityService.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app, IConfiguration configuration)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        EnsureRolesCreated(roleMgr, configuration);

        if (userMgr.Users.Any()) return;

        var rootUserFullName = configuration["RootUser:FullName"]!;
        var rootUserEmail = configuration["RootUser:Email"]!;
        var rootUserPassword = configuration["RootUser:Password"]!;

        var rootUser = userMgr.FindByNameAsync(rootUserEmail).Result;
        if (rootUser == null)
        {
            rootUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = rootUserEmail,
                Email = rootUserEmail,
                EmailConfirmed = true
            };
            var result = userMgr.CreateAsync(rootUser, rootUserPassword).Result;
            if (!result.Succeeded)
            {
                Log.Error("Root user creating failed.");
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddToRoleAsync(rootUser, "admin").Result;
            if (!result.Succeeded)
            {
                Log.Error("Root user creating failed.");
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(rootUser, new Claim[]{
                new(JwtClaimTypes.Name, rootUserFullName),
                new(JwtClaimTypes.Role, "admin")
            }).Result;

            if (!result.Succeeded)
            {
                Log.Error("Root user creating failed.");
                throw new Exception(result.Errors.First().Description);
            }
            Log.Debug("Root user created.");
        }
        else
        {
            Log.Debug("Root user already exists.");
        }
    }

    private static void EnsureRolesCreated(RoleManager<IdentityRole> roleMgr, IConfiguration configuration)
    {
        var roles = configuration.GetSection("Roles")!.Get<string[]>()!;

        foreach (var roleName in roles)
        {
            var role = roleMgr.FindByNameAsync(roleName).Result;
            if (role == null)
            {
                role = new IdentityRole
                {
                    Name = roleName
                };

                _ = roleMgr.CreateAsync(role).Result;
            }
        }
    }
}
