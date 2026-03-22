namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Data.Seeding;
    using GCommon.Exceptions;
    using Services.Core.Contracts;
    using Services.Models.ApplicationUser;
    using ViewModels.Admin.User;
    using static GCommon.ApplicationConstants;
    using static GCommon.OutputMessages.ApplicationUser;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System.Data;

    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService userService;

        private readonly IMapper mapper;

        public UserManagementController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = this.GetAdminUserId()!;

            IEnumerable<UserManageAllDto> userManageDtos = await userService
                .GetAllManageableUsersAsync(userId);
            IEnumerable<AdminManageUserViewModel> userManageViewModel = mapper
                .Map<IEnumerable<AdminManageUserViewModel>>(userManageDtos);

            ViewData["AllRoles"] = IdentitySeeder.ApplicationRoles;

            return View(userManageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(Guid userId, string role)
        {
            try
            {
                bool assignRoleResult = await userService
                    .AssignRoleToUserAsync(userId, role);
                if (!assignRoleResult)
                {
                    TempData[ErrorTempDataKey] = string.Format(UserRoleAssignmentIdentityErrorMessage, role);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (EntityInputDataFormatException eidfe)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                TempData[ErrorTempDataKey] = string.Format(UserRoleAssignmentFailureMessage, role, "assigning");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(Guid userId, string role)
        {
            try
            {
                bool removeRoleResult = await userService
                    .RemoveRoleFromUserAsync(userId, role);
                if (!removeRoleResult)
                {
                    TempData[ErrorTempDataKey] = string.Format(UserRoleRemoveIdentityErrorMessage, role);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (EntityInputDataFormatException eidfe)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                TempData[ErrorTempDataKey] = string.Format(UserRoleAssignmentFailureMessage, role, "removing");

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                bool deleteResult = await userService
                    .DeleteUserAsync(userId);
                if (!deleteResult)
                {
                    TempData[ErrorTempDataKey] = UserDeleteNotExistMessage;

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (EntityInputDataFormatException eidfe)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                TempData[ErrorTempDataKey] = UserDeleteFailureErrorMessage;

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
