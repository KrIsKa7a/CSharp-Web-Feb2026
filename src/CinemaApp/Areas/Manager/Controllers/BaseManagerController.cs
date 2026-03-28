namespace CinemaApp.Web.Areas.Manager.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Core.Contracts;

    [Area("Manager")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public abstract class BaseManagerController : Controller
    {
        protected readonly IManagerService ManagerService;

        protected BaseManagerController(IManagerService managerService)
        {
            this.ManagerService = managerService;
        }

        protected string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        protected async Task<bool> IsUserManagerAsync(string userId)
        {
            bool isUserManager = await ManagerService
                .IsUserManagerAsync(userId);

            return isUserManager;
        }
    }
}
