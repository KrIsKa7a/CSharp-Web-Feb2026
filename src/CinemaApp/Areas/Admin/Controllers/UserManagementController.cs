namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Services.Core.Contracts;
    using Services.Models.ApplicationUser;
    using ViewModels.Admin.User;

    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using Data.Seeding;

    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService userService;

        private readonly IMapper mapper;

        public UserManagementController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

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
    }
}
