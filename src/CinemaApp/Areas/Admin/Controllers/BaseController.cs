namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public abstract class BaseController : Controller
    {
        
    }
}
