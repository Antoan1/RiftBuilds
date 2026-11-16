using Microsoft.AspNetCore.Identity;

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
        }
    }
}