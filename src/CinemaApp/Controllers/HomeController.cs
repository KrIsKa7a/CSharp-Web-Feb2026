namespace CinemaApp.Web.Controllers
{
    using System.Diagnostics;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [ServiceFilter(typeof(PhoneRequiredActionFilter))]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            switch (statusCode)
            {
                case StatusCodes.Status400BadRequest:
                    return View("BadRequest");
                case StatusCodes.Status403Forbidden:
                    return View("Forbidden");
                case StatusCodes.Status404NotFound:
                    return View("NotFound");
                case null:
                case StatusCodes.Status500InternalServerError:
                    return View("ServerError");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
