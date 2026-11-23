namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using Data.Seeding.Utilities;
    using ViewModels;
    using static GCommon.ApplicationConstants;

    public class UserManagement : BaseAdminController
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagement(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> AssignRole(UserRoleInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityUser? dbUser = await userManager.FindByIdAsync(inputModel.UserId);
            if (dbUser == null)
            {
                TempData[ErrorNotificationKey] = "The user you are trying to modify no longer exist!";
                return RedirectToAction(nameof(Index));
            }

            IdentityRole? dbRole = await roleManager.FindByNameAsync(inputModel.RoleName);
            if (dbRole == null)
            {
                TempData[ErrorNotificationKey] = "The role you are trying to assign no longer exist!";
                return RedirectToAction(nameof(Index));
            }

            bool userAlreadyInRole = await userManager.IsInRoleAsync(dbUser, inputModel.RoleName);
            if (userAlreadyInRole)
            {
                TempData[WarningNotificationKey] = "User is already in role!";
                return RedirectToAction(nameof(Index));
            }

            IdentityResult result = await userManager.AddToRoleAsync(dbUser, inputModel.RoleName);
            if (!result.Succeeded)
            {
                TempData[ErrorNotificationKey] = "Unexpected error occurred! Please try again later!";
                return RedirectToAction(nameof(Index));
            }

            TempData[SuccessNotificationKey] = "User role assigned successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(UserRoleInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityUser? dbUser = await userManager.FindByIdAsync(inputModel.UserId);
            if (dbUser == null)
            {
                TempData[ErrorNotificationKey] = "The user you are trying to modify no longer exist!";
                return RedirectToAction(nameof(Index));
            }

            IdentityRole? dbRole = await roleManager.FindByNameAsync(inputModel.RoleName);
            if (dbRole == null)
            {
                TempData[ErrorNotificationKey] = "The role you are trying to remove no longer exist!";
                return RedirectToAction(nameof(Index));
            }

            bool userAlreadyInRole = await userManager.IsInRoleAsync(dbUser, inputModel.RoleName);
            if (!userAlreadyInRole)
            {
                TempData[WarningNotificationKey] = "User was not in the role that you tried to remove!";
                return RedirectToAction(nameof(Index));
            }

            IdentityResult result = await userManager.RemoveFromRoleAsync(dbUser, inputModel.RoleName);
            if (!result.Succeeded)
            {
                TempData[ErrorNotificationKey] = "Unexpected error occurred! Please try again later!";
                return RedirectToAction(nameof(Index));
            }

            TempData[SuccessNotificationKey] = "User role removed successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromForm] string? userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest();
            }

            IdentityUser? dbUser = await userManager.FindByIdAsync(userId);
            if (dbUser == null)
            {
                TempData[ErrorNotificationKey] = "The user you are trying to modify no longer exist!";
                return RedirectToAction(nameof(Index));
            }

            IdentityResult result = await userManager.DeleteAsync(dbUser);
            if (!result.Succeeded)
            {
                TempData[ErrorNotificationKey] = "Unexpected error occurred! Please try again later!";
                return RedirectToAction(nameof(Index));
            }

            TempData[SuccessNotificationKey] = "User deleted successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
