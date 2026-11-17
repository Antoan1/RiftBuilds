using Microsoft.AspNetCore.Identity;
using RiftBuilds.Models;

namespace RiftBuilds.Data
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "User", "Administrator" };

            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    continue;
                }

                var result = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Could not create role '{roleName}': {errors}");
                }
            }

            var configuration = serviceProvider
                .GetRequiredService<IConfiguration>();

            var administratorId = configuration["AdminBootstrap:UserId"];

            if (string.IsNullOrWhiteSpace(administratorId))
            {
                return;
            }

            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByIdAsync(
                administratorId.Trim());

            if (user == null)
            {
                throw new InvalidOperationException(
                    "The configured administrator account does not exist.");
            }

            if (await userManager.IsInRoleAsync(user, "Administrator"))
            {
                return;
            }

            var roleResult = await userManager.AddToRoleAsync(
                user,
                "Administrator");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    "; ",
                    roleResult.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Could not assign the Administrator role: {errors}");
            }
        }
    }
}