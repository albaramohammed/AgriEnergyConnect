using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Farmer", "Employee" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var user = new IdentityUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
            };

            string userPassword = "Admin@123";
            var _user = await userManager.FindByEmailAsync("admin@admin.com");

            if (_user == null)
            {
                var createPowerUser = await userManager.CreateAsync(user, userPassword);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                }
            }
        }
    }
}