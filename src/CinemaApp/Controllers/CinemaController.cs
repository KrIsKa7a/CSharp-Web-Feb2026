namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Cinema.Response;
    using ViewModels.Cinema;

    public class CinemaController : BaseController
    {
        private readonly ICinemaService cinemaService;

        public CinemaController(ICinemaService cinemaService)
        {
            this.cinemaService = cinemaService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<GetAllCinemasResponseModel> responseData = await cinemaService
                .GetAllCinemasAsync();
            IEnumerable<CinemaIndexViewModel> indexViewModel = responseData
                .Select(c => new CinemaIndexViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Location
                })
                .ToArray();

            return View(indexViewModel);
        }
    }
}
