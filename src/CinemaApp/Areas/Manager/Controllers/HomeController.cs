namespace CinemaApp.Web.Areas.Manager.Controllers
{
    using Services.Core.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseManagerController
    {
        public HomeController(IManagerService managerService) 
            : base(managerService)
        {

        }

        public async Task<IActionResult> Index()
        {
            string userId = GetUserId()!;
            
            // TODO: Implement Middleware/Attribute to perform the check for each Action of Manager area
            bool isManager = await IsUserManagerAsync(userId);
            if (!isManager)
            {
                return Unauthorized();
            }

            return View();
        }
    }
}
