namespace CinemaApp.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseManagerController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
