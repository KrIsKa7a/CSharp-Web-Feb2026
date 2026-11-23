namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Data.Seeding.Utilities;
    using ViewModels;

    public class UserManagement : BaseAdminController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagement(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementEntryViewModel> indexViewModel = await userManager
                .Users
                .AsSplitQuery()
                .Select(u => new UserManagementEntryViewModel()
                {
                    Id = u.Id,
                    Email = u.Email ?? "Email not set. Please check user record!",
                })
                .ToArrayAsync();
            foreach (UserManagementEntryViewModel userVm in indexViewModel)
            {
                IdentityUser identityUser = (await userManager.FindByIdAsync(userVm.Id))!;
                userVm.Roles = await userManager.GetRolesAsync(identityUser);
            }

            ViewData["ApplicationRoles"] = IdentitySeeder.DefaultRoles;

            return View(indexViewModel);
        }
    }
}
