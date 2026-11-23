namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static GCommon.ApplicationConstants;

    [Area("Admin")]
    [Authorize(Roles = AdminRoleName)]
    //[AutoValidateAntiforgeryToken]
    public abstract class BaseAdminController : Controller
    {
        
    }
}
