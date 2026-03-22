namespace CinemaApp.Data.Seeding
{
    using Contracts;
    using static GCommon.ExceptionMessages;
    using Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    public class IdentitySeeder : IIdentitySeeder
    {
        // ReSharper disable once InconsistentNaming
        public static string[] ApplicationRoles = new[]
        {
            "Admin",
            "Moderator"
        };

        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task SeedRolesAsync()
        {
            foreach (string role in ApplicationRoles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    IdentityRole<Guid> newRole = new IdentityRole<Guid>(role);

                    IdentityResult identityRoleResult = 
                        await roleManager.CreateAsync(newRole);
                    if (!identityRoleResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            string.Format(RoleSeedingExceptionMessage, role));
                    }
                }
            }
        }

        public async Task SeedAdminUserAsync()
        {
            string adminEmail = configuration["UserSeed:AdminAccount:Email"] ?? 
                                throw new InvalidOperationException(AdminUserSeedingEmailNotFoundMessage);
            string adminPassword = configuration["UserSeed:AdminAccount:Password"] ??
                                   throw new InvalidOperationException(AdminUserSeedingPasswordNotFoundMessage);

            ApplicationUser? adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Birthdate = DateTime.Now.AddYears(-18),
                };

                IdentityResult result = await userManager
                    .CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(AdminUserSeedingExceptionMessage);
                }
            }

            bool isInRole = await userManager
                .IsInRoleAsync(adminUser, ApplicationRoles[0]);
            if (!isInRole)
            {
                IdentityResult result = await userManager
                    .AddToRoleAsync(adminUser, ApplicationRoles[0]);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(AdminUserSeedingExceptionMessage);
                }
            }
        }
    }
}
