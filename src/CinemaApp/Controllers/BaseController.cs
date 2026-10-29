namespace CinemaApp.Web.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public abstract class BaseController : Controller
    {
        protected string? GetUserId
            => User.FindFirstValue(ClaimTypes.NameIdentifier);

        protected string? GetUserEmail
            => User.FindFirstValue(ClaimTypes.Email);

        protected bool IsUserAuthenticated
            => User.Identity?.IsAuthenticated ?? false;
    }
}
