namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Contracts;
    using Services.Core.Data.Cinema.Response;
    using ViewModels.Cinema;
    using ViewModels.CinemaMovie;
    using static GCommon.ApplicationConstants;

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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Program([FromRoute] Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            CinemaProgramResponseModel? responseData = await cinemaService
                .GetCinemaProgramByIdAsync(id.Value);
            if (responseData == null)
            {
                return NotFound();
            }

            IEnumerable<CinemaProgramViewModel> programViewModel = responseData.Movies
                .Select(m => new CinemaProgramViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                })
                .ToArray();

            return View(programViewModel);
        }
    }
}
