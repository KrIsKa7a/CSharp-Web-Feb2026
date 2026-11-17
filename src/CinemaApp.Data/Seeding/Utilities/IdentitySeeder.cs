namespace CinemaApp.Data.Seeding.Utilities
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class IdentitySeeder
    {
        public static string[] DefaultRoles = new[] { "User", "Manager", "Admin" };

        public static void SeedRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            foreach (string roleName in DefaultRoles)
            {
                bool roleExists = roleManager.RoleExistsAsync(roleName)
                    .GetAwaiter()
                    .GetResult();
                if (roleExists)
                {
                    continue;
                }

                IdentityRole identityRole = new IdentityRole(roleName);
                IdentityResult result = roleManager.CreateAsync(identityRole)
                    .GetAwaiter()
                    .GetResult();
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Role {roleName} can't be created!");
                }
            }
        }
    }
}
