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

        public static void SeedAdminUser(IServiceProvider serviceProvider, string adminEmail, string adminPassword)
        {
            UserManager<IdentityUser> userManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser? adminUser = userManager.FindByNameAsync(adminEmail)
                .GetAwaiter()
                .GetResult();
            if (adminUser != null)
            {
                return;
            }

            adminUser = new IdentityUser(adminEmail)
            {
                Email = adminEmail
            };

            IdentityResult result = userManager.CreateAsync(adminUser, adminPassword)
                .GetAwaiter()
                .GetResult();
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Admin user {adminEmail} can't be seeded!");
            }

            result = userManager.AddToRoleAsync(adminUser, DefaultRoles[2])
                .GetAwaiter()
                .GetResult();
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Admin user {adminEmail} can't be added to role {DefaultRoles[2]}!");
            }
        }
    }
}
